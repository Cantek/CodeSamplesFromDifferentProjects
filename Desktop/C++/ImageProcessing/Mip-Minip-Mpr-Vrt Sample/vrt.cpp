#include "vrt.h"

#include "vrtpreset.h"
#include "childwnd3d.h"
#include "Color3D.h"

VRT::VRT(ChildWnd3D *parent)
	:Abstract3D(parent)
	,textureMapper(NULL)
	,colorFunction(NULL)
{

}

VRT::~VRT(void)
{
	if(textureMapper)
	{
		//textureMapper->SetInput(NULL);
		textureMapper->RemoveAllInputs();
		textureMapper->RemoveAllClippingPlanes();
		textureMapper->Delete();
		textureMapper=NULL;
	}
	if(colorFunction)
	{
		colorFunction->RemoveAllPoints();
		colorFunction->Delete();
		colorFunction=NULL;
	}
}
void VRT::createVTKPipeline()
{

	opacityFunction=vtkPiecewiseFunction::New();
	opacityFunction->ClampingOn();
	opacityFunction->AddPoint(this->childWindow->getRangeMin(), 0);
	opacityFunction->AddPoint(97, 0);
    opacityFunction->AddPoint(229, 1);
    opacityFunction->AddPoint(2087, 1);
    opacityFunction->AddPoint(2487, 1);
	opacityFunction->AddPoint(this->childWindow->getRangeMax(), 0);
    opacityFunction->Update();

	Color3D *color1=new Color3D(142, 56, 56);
	Color3D *color2=new Color3D(156, 108, 80);
	Color3D *color3=new Color3D(242, 214, 214);

	colorFunction=vtkColorTransferFunction::New();
	colorFunction->AddRGBPoint(129,color1->getRed(),color1->getGreen(),color1->getBlue());
	colorFunction->AddRGBPoint(250,color2->getRed(),color2->getGreen(),color2->getBlue());
	colorFunction->AddRGBPoint(1900,color3->getRed(),color3->getGreen(),color3->getBlue());

	volumeProperty = vtkVolumeProperty::New();
	volumeProperty->SetColor(colorFunction);
	volumeProperty->SetScalarOpacity(opacityFunction);
	volumeProperty->SetInterpolationTypeToLinear();
	volumeProperty->ShadeOn();
    volumeProperty->SetSpecular(0.69);
    volumeProperty->SetAmbient(0.5);
    volumeProperty->SetDiffuse(0.5);
    volumeProperty->SetSpecularPower(1);

	textureMapper=vtkVolumeTextureMapper3D::New();
	textureMapper->SetBlendModeToComposite();
	textureMapper->SetPreferredMethodToNVidia();
	textureMapper->SetSampleDistance(0.5);
	textureMapper->SetInputConnection(imageChange->GetOutputPort());
	

	int valid=textureMapper->IsRenderSupported(volumeProperty);
	/*if(!valid)
		textureMapper->SetPreferredMethodToFragmentProgram();*/
	volume = vtkVolume::New();
	volume->SetProperty(volumeProperty);
	volume->SetMapper(textureMapper);

	this->childWindow->GetRenderer3D()->AddVolume(volume);

	
	this->setBoxWidget();
	this->setOutlineActor();
	this->setOrientationMarker();
	this->setAnnotationActor();
	//this->setClipPlaneVisibility(0);
	//this->setOutlineVisibility(0);

	delete color1,color2,color3;
	//colorFunction->Delete();
	//opacityFunction->Delete();
	//volumeProperty->Delete();
	//textureMapper->Delete();
	//volume->Delete();

	/*double orientation[3];
	volume->GetOrientation(orientation);*/

	this->childWindow->GetRenderer3D()->ResetCameraClippingRange();
}
void VRT::presetChanged(VRTPreset *preset)
{
	//preset->printSelf(0);
	//opacityFunction=vtkPiecewiseFunction::New();
	opacityFunction->RemoveAllPoints();
	opacityFunction->ClampingOn();
	opacityFunction->AddPoint(childWindow->getRangeMin(),0);
	opacityFunction->AddPoint(convertPVToHU(preset->getOpacityScalars()[0]),preset->getOpacityPercents()[0]);
	opacityFunction->AddPoint(convertPVToHU(preset->getOpacityScalars()[1]),preset->getOpacityPercents()[1]);
	opacityFunction->AddPoint(convertPVToHU(preset->getOpacityScalars()[2]),preset->getOpacityPercents()[2]);
	opacityFunction->AddPoint(convertPVToHU(preset->getOpacityScalars()[3]),preset->getOpacityPercents()[3]);
	opacityFunction->AddPoint(childWindow->getRangeMax(),0);
	opacityFunction->Update();
	
	Color3D *color1=new Color3D(preset->getColors3D()->at(0));
		
	Color3D *color2=new Color3D(preset->getColors3D()->at(1));

	Color3D *color3=new Color3D(preset->getColors3D()->at(2));

	//colorFunction=vtkColorTransferFunction::New();
	colorFunction->RemoveAllPoints();
	colorFunction->AddRGBPoint(convertPVToHU(preset->getColorScalars()[0]),color1->getRed(),color1->getGreen(),color1->getBlue());
	colorFunction->AddRGBPoint(convertPVToHU(preset->getColorScalars()[1]),color2->getRed(),color2->getGreen(),color2->getBlue());
	colorFunction->AddRGBPoint(convertPVToHU(preset->getColorScalars()[2]),color3->getRed(),color3->getGreen(),color3->getBlue());
	
	//volumeProperty = vtkVolumeProperty::New();
	volumeProperty->SetColor(colorFunction);
	volumeProperty->SetScalarOpacity(opacityFunction);
	volumeProperty->SetAmbient((double)preset->getLightAmbient()/100.0);
	volumeProperty->SetDiffuse((double)preset->getLightDiffuse()/100.0);
	volumeProperty->SetSpecular((double)preset->getLightSpecular()/100.0);
	volumeProperty->SetSpecularPower((double)preset->getLightIntensity()/100.0);
	volumeProperty->SetInterpolationTypeToLinear();
	volumeProperty->ShadeOn();

	volume->SetProperty(volumeProperty);
	
	//childWindow->GetRenderer3D()->UpdateLights();
	childWindow->GetRenderer3D()->ResetCameraClippingRange();
	childWindow->GetRenderWindow()->Render();
}
void VRT::setQualityType(int quality)
{
	switch(quality)
	{
	case LOW_QUALITY:
		volumeProperty->SetInterpolationTypeToNearest();
		textureMapper->SetSampleDistance(1);
		break;
	case MEDIUM_QUALITY:
		volumeProperty->SetInterpolationTypeToLinear();
		textureMapper->SetSampleDistance(0.7);
		break;
	case HIGH_QUALITY:
		volumeProperty->SetInterpolationTypeToLinear();
		textureMapper->SetSampleDistance(0.1);
		break;
	}
	this->render();
}
void VRT::clipPlanes(vtkPlanes *planes)
{
	this->textureMapper->SetClippingPlanes(planes);
}
