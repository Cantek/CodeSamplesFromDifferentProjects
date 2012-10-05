// CannyEdgeDetection.cpp : Defines the entry point for the console application.
//
#include "stdafx.h"
#include "itkRescaleIntensityImageFilter.h"
#include "itkCannyEdgeDetectionImageFilter.h"
#include "itkCastImageFilter.h"
#include "itkImageFileReader.h"
#include "itkImageFileWriter.h"
#include "itkVTKImageImport.h"
#include "itkVTKImageExport.h"
#include "itkRescaleIntensityImageFilter.h"
#include "itkImageToVtkImageFilter.h"
#include "vtkImageExport.h"
#include "vtkImageImport.h"
#include "vtkRenderer.h"
#include "vtkRenderWindow.h"
#include "vtkImageActor.h"
#include "vtkPolyDataMapper.h"
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

int _tmain(int argc, _TCHAR* argv[])
{
	float variance=2.0;

	float upperThreshold = 0.0;

	float lowerThreshold = 0.0;

	typedef unsigned char CharPixelType;

	typedef double RealPixelType;

	const int Dimension = 2;

	typedef itk::Image<CharPixelType,Dimension> CharImageType;

	typedef itk::Image<RealPixelType,Dimension> RealImageType;

	typedef itk::ImageFileReader<CharImageType> ReaderType;

	typedef itk::ImageFileWriter<CharImageType> WriterType;

	typedef itk::CastImageFilter<CharImageType,RealImageType> CastFilterType;

	typedef itk::CannyEdgeDetectionImageFilter<RealImageType,RealImageType> CannyFilter;

	typedef itk::RescaleIntensityImageFilter<RealImageType,CharImageType> RescaleFilter;

	RescaleFilter::Pointer rescaleFilter = RescaleFilter::New();

	ReaderType::Pointer reader=ReaderType::New();

	WriterType::Pointer writer = WriterType::New();

	CastFilterType::Pointer castFilter = CastFilterType::New();

	CannyFilter::Pointer cannyFilter = CannyFilter::New();

	reader->SetFileName("C:\\Users\\circass\\Desktop\\YEDEKLERRRR\\MANIX\\CER-CT\\SANS I.V\\IM-0001-0001.dcm");

	castFilter->SetInput(reader->GetOutput());

	castFilter->Update();

	cannyFilter->SetInput(castFilter->GetOutput());

	cannyFilter->SetVariance(variance);

	cannyFilter->SetUpperThreshold(upperThreshold);

	cannyFilter->SetLowerThreshold(lowerThreshold);

	rescaleFilter->SetInput(cannyFilter->GetOutput());

	rescaleFilter->Update();

	typedef itk::VTKImageExport< CharImageType > ExportFilterType;
	ExportFilterType::Pointer itkExporter = ExportFilterType::New();

	itkExporter->SetInput( rescaleFilter->GetOutput() );

	vtkImageImport* vtkImporter = vtkImageImport::New();  
	ConnectPipelines(itkExporter, vtkImporter;)
	vtkRenderWindow *renWin = vtkRenderWindow::New();

	vtkRenderer *renderer = vtkRenderer::New();

	vtkImageActor *actor = vtkImageActor::New();

	vtkRenderWindowInteractor *renWinInt  = vtkRenderWindowInteractor::New();

	actor->SetInput(vtkImporter->GetOutput());
	
	renderer->AddActor(actor);

	renWin->AddRenderer(renderer);

	renWin->SetInteractor(renWinInt);

	renWin->Render();

	renWinInt->Start();

	return 0;
}