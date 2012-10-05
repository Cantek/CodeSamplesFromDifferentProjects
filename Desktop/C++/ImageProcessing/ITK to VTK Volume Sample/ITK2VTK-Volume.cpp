#if defined(_MSC_VER)
#pragma warning ( disable : 4786 )
#endif

#ifdef __BORLANDC__
#define ITK_LEAN_AND_MEAN
#endif

#include "itkOrientedImage.h"
#include "itkGDCMImageIO.h"
#include "itkGDCMSeriesFileNames.h"
#include "itkImageSeriesReader.h"
#include "itkImageFileWriter.h"
#include "itkCommand.h"
#include "itkImage.h"
#include "itkVTKImageExport.h"
#include "itkOrientedImage.h"
#include "itkGDCMImageIO.h"
#include "itkGDCMSeriesFileNames.h"
#include "itkImageSeriesReader.h"
#include "itkImageFileWriter.h"
#include "itkVTKImageImport.h"
#include "itkCurvatureFlowImageFilter.h"
#include "itkCastImageFilter.h"
#include "itkRGBPixel.h"
#include "itkImageFileReader.h"
#include "itkImageFileWriter.h"
#include "itkDICOMImageIO2Factory.h"
#include "itkDICOMImageIO2.h"
#include "itkImageSeriesReader.h"
#include "itkDICOMSeriesFileNames.h"
#include "vtkColorTransferFunction.h"
#include "vtkImageData.h"
#include "vtkImageImport.h"
#include "vtkImageExport.h"
#include "vtkImageActor.h"
#include "vtkRenderer.h"
#include "vtkRenderWindow.h"
#include "vtkVolumeProperty.h"
#include "vtkVolumeTextureMapper.h"
#include "vtkVolumeTextureMapper3D.h"
#include "vtkFixedPointVolumeRayCastMapper.h";
#include "vtkRenderWindowInteractor.h"
#include "vtkInteractorStyleImage.h" 
#include "vtkPieceWiseFunction.h" 
#include "conio.h"


template <typename ITK_Exporter, typename VTK_Importer>
void ConnectPipelines(ITK_Exporter exporter, VTK_Importer* importer)
{
	importer->SetUpdateInformationCallback(exporter->GetUpdateInformationCallback());
	importer->SetPipelineModifiedCallback(exporter->GetPipelineModifiedCallback());
	importer->SetWholeExtentCallback(exporter->GetWholeExtentCallback());
	importer->SetSpacingCallback(exporter->GetSpacingCallback());
	importer->SetOriginCallback(exporter->GetOriginCallback());
	importer->SetScalarTypeCallback(exporter->GetScalarTypeCallback());
	importer->SetNumberOfComponentsCallback(exporter->GetNumberOfComponentsCallback());
	importer->SetPropagateUpdateExtentCallback(exporter->GetPropagateUpdateExtentCallback());
	importer->SetUpdateDataCallback(exporter->GetUpdateDataCallback());
	importer->SetDataExtentCallback(exporter->GetDataExtentCallback());
	importer->SetBufferPointerCallback(exporter->GetBufferPointerCallback());
	importer->SetCallbackUserData(exporter->GetCallbackUserData());
}

template <typename VTK_Exporter, typename ITK_Importer>
void ConnectPipelines(VTK_Exporter* exporter, ITK_Importer importer)
{
	importer->SetUpdateInformationCallback(exporter->GetUpdateInformationCallback());
	importer->SetPipelineModifiedCallback(exporter->GetPipelineModifiedCallback());
	importer->SetWholeExtentCallback(exporter->GetWholeExtentCallback());
	importer->SetSpacingCallback(exporter->GetSpacingCallback());
	importer->SetOriginCallback(exporter->GetOriginCallback());
	importer->SetScalarTypeCallback(exporter->GetScalarTypeCallback());
	importer->SetNumberOfComponentsCallback(exporter->GetNumberOfComponentsCallback());
	importer->SetPropagateUpdateExtentCallback(exporter->GetPropagateUpdateExtentCallback());
	importer->SetUpdateDataCallback(exporter->GetUpdateDataCallback());
	importer->SetDataExtentCallback(exporter->GetDataExtentCallback());
	importer->SetBufferPointerCallback(exporter->GetBufferPointerCallback());
	importer->SetCallbackUserData(exporter->GetCallbackUserData());
}

int main( int argc, char* argv[] )
{
	typedef unsigned short    PixelType;
	const unsigned int      Dimension = 3;

	typedef itk::OrientedImage< PixelType, Dimension >         ImageType;
	typedef itk::ImageSeriesReader< ImageType >        ReaderType;
	ReaderType::Pointer reader = ReaderType::New();
	typedef itk::GDCMImageIO       ImageIOType;
	ImageIOType::Pointer dicomIO = ImageIOType::New();

	reader->SetImageIO( dicomIO );
	typedef itk::GDCMSeriesFileNames NamesGeneratorType;
	NamesGeneratorType::Pointer nameGenerator = NamesGeneratorType::New();

	nameGenerator->SetUseSeriesDetails( true );
	nameGenerator->SetDirectory( "C:\\Users\\circass\\Desktop\\YEDEKLERRRR\\MANIX\\CER-CT\\SANS I.V" );

	std::cout << std::endl << "The directory: " << std::endl;
	std::cout << std::endl << "C:\\Users\\circass\\Desktop\\YEDEKLERRRR\\MANIX\\CER-CT\\SANS I.V" << std::endl << std::endl;
	std::cout << "Contains the following DICOM Series: ";
	std::cout << std::endl << std::endl;

	typedef std::vector< std::string >    SeriesIdContainer;

	const SeriesIdContainer & seriesUID = nameGenerator->GetSeriesUIDs();

	SeriesIdContainer::const_iterator seriesItr = seriesUID.begin();
	SeriesIdContainer::const_iterator seriesEnd = seriesUID.end();
	while( seriesItr != seriesEnd )
	{
		std::cout << seriesItr->c_str() << std::endl;
		seriesItr++;
	}
	std::string seriesIdentifier;
	seriesIdentifier = seriesUID.begin()->c_str();
	std::cout << std::endl << std::endl;
	std::cout << "Now reading series: " << std::endl << std::endl;
	std::cout << seriesIdentifier << std::endl;
	std::cout << std::endl << std::endl;

	typedef std::vector< std::string >   FileNamesContainer;
	FileNamesContainer fileNames;
	fileNames = nameGenerator->GetFileNames( seriesIdentifier );

	reader->SetFileNames( fileNames );
	reader->Update();   

	typedef itk::VTKImageExport< ImageType > ExportFilterType;
	ExportFilterType::Pointer itkExporter = ExportFilterType::New();

	itkExporter->SetInput( reader->GetOutput() );

	vtkImageImport* vtkImporter = vtkImageImport::New();  
	ConnectPipelines(itkExporter, vtkImporter);

	typedef itk::VTKImageImport< ImageType > ImportFilterType;
	ImportFilterType::Pointer itkImporter = ImportFilterType::New();

	vtkImageExport* vtkExporter = vtkImageExport::New();  
	ConnectPipelines(vtkExporter, itkImporter);
	vtkExporter->SetInput( vtkImporter->GetOutput() );

	vtkPiecewiseFunction* opacityTransferFunction = vtkPiecewiseFunction::New();
	opacityTransferFunction->AddPoint(0.9,0.1);
	opacityTransferFunction->AddPoint(2250,0);

	vtkColorTransferFunction* colorTransferFunction = vtkColorTransferFunction::New();  

	vtkVolume* volume = vtkVolume::New();
	vtkVolumeProperty* volumeProperty = vtkVolumeProperty::New(); 
	//vtkVolumeTextureMapper3D* volumeMapperSoftware = vtkVolumeTextureMapper3D::New();
	vtkFixedPointVolumeRayCastMapper* volumeMapperSoftware = vtkFixedPointVolumeRayCastMapper::New();

	volumeProperty->SetScalarOpacity(opacityTransferFunction);
	volumeProperty->SetInterpolationTypeToNearest();
	volumeProperty->ShadeOff(); 
	volume->SetProperty(volumeProperty); 

	volumeMapperSoftware->SetInput(vtkImporter->GetOutput());
	volumeMapperSoftware->SetSampleDistance(0.1);
	volume->SetMapper(volumeMapperSoftware);

	vtkInteractorStyleTrackballCamera * interactorStyle = vtkInteractorStyleTrackballCamera::New();

	vtkRenderer* renderer = vtkRenderer::New();
	vtkRenderWindow* renWin = vtkRenderWindow::New();
	//renderer->SetBackground(1,1,1);
	vtkRenderWindowInteractor* iren = vtkRenderWindowInteractor::New();

	renWin->SetSize(500, 500);
	renWin->AddRenderer(renderer);
	iren->SetRenderWindow(renWin);
	iren->SetInteractorStyle(interactorStyle);

	renderer->AddVolume(volume);
	renWin->Render();
	iren->Start();
}
