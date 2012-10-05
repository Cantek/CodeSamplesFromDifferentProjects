#include "mip.h"
#include "controller3d.h"
#include "childwnd3d.h"

#include <math.h>
MIP::MIP(ChildWnd3D *parent)
	: Abstract3D(parent)
	,raycastMapper(NULL)
	,imageShiftScale(NULL)
	,grayFunction(NULL)
{
	connect(controller3d,SIGNAL(sendWindowLevel(int,int)),this,SLOT(windowLevelChanged(int,int)));
}

MIP::~MIP()
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
void MIP::createVTKPipeline()
{
	
	double min = childWindow->getRangeMin();
	double max = childWindow->getRangeMax();
	double diff = max-min;
	double slope = 255.0/diff;
	double inter = -slope*min;
	double shift = inter/slope;

	imageShiftScale = vtkImageShiftScale::New();
	imageShiftScale->SetInputConnection(imageChange->GetOutputPort());
	imageShiftScale->SetShift(shift);
	imageShiftScale->SetScale(slope);
	imageShiftScale->SetOutputScalarTypeToUnsignedShort();
	imageShiftScale->Update();   
	double range[2];
	imageShiftScale->GetOutput()->GetScalarRange(range);
	double level = 0.5 * (range[1] + range[0]);
	double window = range[1] - range[0];
	double *spacing=childWindow->getSpacing();
	double pix_diag = sqrt(spacing[0] * spacing[0] + spacing[1] * spacing[1] + spacing[2] * spacing[2]);
	
	opacityFunction=vtkPiecewiseFunction::New();
	opacityFunction->AddPoint(level-window/2,0.0);
	opacityFunction->AddPoint(level+window/2,1.0);

	grayFunction=vtkPiecewiseFunction::New();
	grayFunction->AddSegment( level - window/2, 0.0 , level + window/2, 1.0 );

	volumeProperty=vtkVolumeProperty::New();
	volumeProperty->SetScalarOpacity(opacityFunction);
	volumeProperty->SetColor(grayFunction);
	volumeProperty->SetInterpolationTypeToLinear();
	volumeProperty->SetScalarOpacityUnitDistance(pix_diag);

	vtkVolumeRayCastMIPFunction* mipFunction = vtkVolumeRayCastMIPFunction::New();
	mipFunction->SetMaximizeMethodToOpacity();

	raycastMapper=vtkVolumeRayCastMapper::New();
	raycastMapper->SetVolumeRayCastFunction(mipFunction);
	raycastMapper->SetInputConnection(imageShiftScale->GetOutputPort());
	raycastMapper->SetImageSampleDistance(0.25);
	raycastMapper->SetSampleDistance(pix_diag/5.0);

	volume=vtkVolume::New();
	volume->SetMapper(raycastMapper);
	volume->SetProperty(volumeProperty);

	childWindow->GetRenderer3D()->AddViewProp(volume);

	this->setBoxWidget();
	this->setOutlineActor();
	this->setOrientationMarker();
	this->setAnnotationActor();

	childWindow->GetRenderer3D()->ResetCameraClippingRange();
	childWindow->GetRenderer3D()->ResetCamera();

	mipFunction->Delete();
	controller3d->windowLevelDefaultValues(window,level,MIP_3D);
	controller3d->lightsDefault(volumeProperty->GetAmbient()*100,
		volumeProperty->GetDiffuse()*100,
		volumeProperty->GetSpecular()*100,
		volumeProperty->GetSpecularPower()*100);

}
void MIP::presetChanged(VRTPreset *preset)
{
}
void MIP::setQualityType(int quality)
{
	switch(quality)
	{
	case LOW_QUALITY:
		volumeProperty->SetInterpolationTypeToNearest();
		raycastMapper->SetSampleDistance(1);
		break;
	case MEDIUM_QUALITY:
		volumeProperty->SetInterpolationTypeToLinear();
		raycastMapper->SetSampleDistance(0.7);
		break;
	case HIGH_QUALITY:
		volumeProperty->SetInterpolationTypeToLinear();
		raycastMapper->SetSampleDistance(0.1);
		break;
	}
	this->render();
}
void MIP::windowLevelChanged(int window, int level)
{
	opacityFunction->RemoveAllPoints();
	opacityFunction->AddPoint(level-window/2,0.0);
	opacityFunction->AddPoint(level+window/2,1.0);

	grayFunction->RemoveAllPoints();
	grayFunction->AddSegment( level - window/2, 0.0 , level + window/2, 1.0 );
	
	volumeProperty->SetScalarOpacity(opacityFunction);
	volumeProperty->SetColor(grayFunction);
	
	volume->Update();
	this->render();
}
void MIP::clipPlanes(vtkPlanes *planes)
{
	this->raycastMapper->SetClippingPlanes(planes);
}
