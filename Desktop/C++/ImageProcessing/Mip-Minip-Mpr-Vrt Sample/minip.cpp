#include "minip.h"
#include "controller3d.h"
#include "childwnd3d.h"

#include <math.h>

MINIP::MINIP(ChildWnd3D *parent)
: Abstract3D(parent)
,raycastMapper(NULL)
,imageShiftScale(NULL)
,grayFunction(NULL)
{
	connect(controller3d,SIGNAL(sendOrientation(int)),this,SLOT(orientationChanged(int)));
}

MINIP::~MINIP()
{
	if(raycastMapper)
	{
		raycastMapper->RemoveAllInputs();
		raycastMapper->RemoveAllClippingPlanes();
		raycastMapper->Delete();
		raycastMapper=NULL;
	}
	if(imageShiftScale)
	{
		imageShiftScale->SetInput(NULL);
		imageShiftScale->RemoveAllInputs();
		imageShiftScale->Delete();
		imageShiftScale=NULL;
	}
	if(grayFunction)
	{
		grayFunction->RemoveAllPoints();
		grayFunction->Delete();
		grayFunction=NULL;
	}
}
void MINIP::createVTKPipeline()
{
	double min = childWindow->getRangeMin();
	double max = childWindow->getRangeMax();
	double diff = max-min;
	double slope = 255.0/diff;
	double inter = -slope*min;
	double shift = inter/slope;

	double window = max - min;
	double level = min + window/2;
	
	double *spacing=childWindow->getSpacing();
	double pix_diag = sqrt(spacing[0] * spacing[0] + spacing[1] * spacing[1] + spacing[2] * spacing[2]);

	opacityFunction=vtkPiecewiseFunction::New();
	//opacityFunction->AddPoint(range[0],0.);
	opacityFunction->AddPoint(level+window/2,0.0);
	opacityFunction->AddPoint(level-window/2,1.0);
	//double ra[]={level-window/2,level+window/2};
	//opacityFunction->AdjustRange(ra);
	//opacityFunction->AddSegment(range[0],0.0,level-window/2,0.0);
	//opacityFunction->AddSegment(level+window/2,0.0,level-window/2,1.0);
	//opacityFunction->AddSegment(level+window/2,0.0,range[1],0.0);
	opacityFunction->ClampingOn();
	opacityFunction->Update();

	grayFunction=vtkPiecewiseFunction::New();
	grayFunction->AddSegment( level - window/2, 0.0 , level + window/2, 1.0 );

	volumeProperty=vtkVolumeProperty::New();
	volumeProperty->SetScalarOpacity(opacityFunction);
	//volumeProperty->SetDisableGradientOpacity(0);
	volumeProperty->SetColor(grayFunction);
	volumeProperty->SetInterpolationTypeToLinear();
	//volumeProperty->SetScalarOpacityUnitDistance(pix_diag);
	//volumeProperty->SetIndependentComponents(0);
	volumeProperty->ShadeOff();

	/*vtkVolumeRayCastMIPFunction* mipFunction = vtkVolumeRayCastMIPFunction::New();
	mipFunction->SetMaximizeMethodToOpacity();*/

	fixedMapper=vtkFixedPointVolumeRayCastMapper::New();
	fixedMapper->SetInputConnection(imageChange->GetOutputPort());
	fixedMapper->SetBlendModeToMinimumIntensity();
	fixedMapper->SetSampleDistance(1.);
	fixedMapper->SetScalarModeToDefault();
	//raycastMapper=vtkVolumeRayCastMapper::New();
	//raycastMapper->SetVolumeRayCastFunction(mipFunction);
	//raycastMapper->SetInputConnection(reslice->GetOutputPort());
	//raycastMapper->SetImageSampleDistance(0.25);
	//raycastMapper->SetSampleDistance(pix_diag/5.0);

	volume=vtkVolume::New();
	volume->SetMapper(fixedMapper);
	volume->SetProperty(volumeProperty);
	childWindow->GetRenderer3D()->SetBackground(1,1,1);
	childWindow->GetRenderer3D()->AddViewProp(volume);
	this->setBoxWidget();
	this->setOutlineActor();
	this->setOrientationMarker();
	this->setAnnotationActor();

	childWindow->GetRenderer3D()->ResetCameraClippingRange();
	childWindow->GetRenderer3D()->ResetCamera();
}
void MINIP::presetChanged(VRTPreset *preset)
{
}
void MINIP::setQualityType(int quality)
{
	switch(quality)
	{
	case LOW_QUALITY:
		volumeProperty->SetInterpolationTypeToNearest();
		fixedMapper->SetSampleDistance(1);
		break;
	case MEDIUM_QUALITY:
		volumeProperty->SetInterpolationTypeToLinear();
		fixedMapper->SetSampleDistance(0.7);
		break;
	case HIGH_QUALITY:
		volumeProperty->SetInterpolationTypeToLinear();
		fixedMapper->SetSampleDistance(0.1);
		break;
	}
	this->render();
}
void MINIP::clipPlanes(vtkPlanes *planes)
{
	this->fixedMapper->SetClippingPlanes(planes);
}