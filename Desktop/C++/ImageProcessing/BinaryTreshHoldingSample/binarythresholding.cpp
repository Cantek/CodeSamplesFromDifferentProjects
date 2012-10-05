#include "binarythresholding.h"
#include "QVTKWidget.h"
#include "itkBinaryThresholdImageFilter.h"
#include "itkImage.h"
#include "itkImageFileReader.h"
#include "itkImageToVTKImageFilter.h"
#include "vtkImageViewer.h"
#include "itkVTKImageImport.h"
#include "vtkImageImport.h"
#include "vtkImageExport.h"
#include "vtkImageActor.h"
#include "vtkRenderer.h"
#include "vtkPolyDataMapper.h"
#include "vtkImageMapper.h"
#include "vtkRenderWindowInteractor.h"




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


BinaryThresholding::BinaryThresholding(QWidget *parent, Qt::WFlags flags)
: QMainWindow(parent, flags)
{
	//ui.setupUi(this);
	typedef  unsigned char  InputPixelType;

	typedef  unsigned char  OutputPixelType;

	typedef itk::Image< InputPixelType,  2 >   InputImageType;

	typedef itk::Image< OutputPixelType, 2 >   OutputImageType;

	typedef itk::BinaryThresholdImageFilter<InputImageType, OutputImageType >  FilterType;

	typedef itk::ImageFileReader<InputImageType> ReaderType;

	ReaderType::Pointer reader= ReaderType::New();

	FilterType::Pointer filter = FilterType::New();

	reader->SetFileName("C:\\Users\\circass\\Desktop\\YEDEKLERRRR\\MANIX\\CER-CT\\SANS I.V\\IM-0001-0001.dcm");

	filter->SetInput(reader->GetOutput());

	float variance = 2.0;

	float upperThreshold = 250.0;

	float lowerThreshold = 100.0;

	float outsideValue = 500.0;

	float insideValue = 175.0;

	filter->SetLowerThreshold( lowerThreshold );

	filter->SetUpperThreshold( upperThreshold );

	filter->SetOutsideValue(outsideValue);

	filter->SetInsideValue(insideValue);

	filter->Update();	

	typedef itk::VTKImageExport< InputImageType > ExportFilterType;
	ExportFilterType::Pointer itkExporter = ExportFilterType::New();

	itkExporter->SetInput( filter->GetOutput() );

	vtkImageImport* vtkImporter = vtkImageImport::New();  
	ConnectPipelines(itkExporter, vtkImporter);

	typedef itk::VTKImageImport< InputImageType > ImportFilterType;
	ImportFilterType::Pointer itkImporter = ImportFilterType::New();

	vtkImageExport* vtkExporter = vtkImageExport::New();  
	ConnectPipelines(vtkExporter, itkImporter);
	vtkExporter->SetInput( vtkImporter->GetOutput() );


	vtkImageViewer* viewer= vtkImageViewer::New();

	viewer->SetColorWindow( 255);

	viewer->SetColorLevel( 128);

	vtkRenderWindowInteractor* renderWindowInteractor=vtkRenderWindowInteractor::New();

	viewer->SetupInteractor( renderWindowInteractor);

	viewer->SetInput( vtkImporter->GetOutput() );

	viewer->Render();

	renderWindowInteractor->Start();

	//ui.qvtkWidget->SetRenderWindow(viewer->GetRenderWindow());
}

BinaryThresholding::~BinaryThresholding()
{

}