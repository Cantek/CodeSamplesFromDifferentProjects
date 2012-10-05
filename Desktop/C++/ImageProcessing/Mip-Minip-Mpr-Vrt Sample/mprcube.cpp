#include "mprcube.h"
#include "akvtk3dwidget.h"
#include "color3d.h"
#include "point3d.h"
#include "mprbase.h"

#include <QString>
#include <vtkCubeSource.h>
#include <vtkPolyDataMapper.h>

#include <vtkProperty.h>
#include <vtkAxesActor.h>
#include <vtkMatrix4x4.h>
#include <vtkLineSource.h>
#include <vtkPlane.h>
#include <vtkCutter.h>


#include <vtkProperty2D.h>
#include <vtkTextProperty.h>

#include <QDebug>

MPRCube::MPRCube(MPRBase *mpr)
	: QObject(mpr)
	,mprbase(mpr)
{
	xyRange=mprbase->getXYViewer()->m_viewer->GetSliceMax();
	xzRange=mprbase->getXZViewer()->m_viewer->GetSliceMax();
	yzRange=mprbase->getYZViewer()->m_viewer->GetSliceMax();
	this->imageBounds=mprbase->getImageBounds();
	//qDebug()<<"Bounds = "<<imageBounds[0]<<", "<<imageBounds[1]<<", "<<imageBounds[2]<<", "<<imageBounds[3]<<", "<<imageBounds[4]<<", "<<imageBounds[5];
}

MPRCube::~MPRCube()
{
	mprbase->getRenderer3D()->RemoveAllViewProps();
	xyActor->Delete();
	xzActor->Delete();
	yzActor->Delete();
	axialText->Delete();
	sagittalText->Delete();
	coronalText->Delete();
}
void MPRCube::createVTKPipeline()
{
	vtkCubeSource *cube = vtkCubeSource::New();
	actor = vtkActor::New();
	cube->SetBounds(imageBounds);
	
	//actor->SetPosition(imageBounds[1]-imageBounds[0],imageBounds[3]-imageBounds[2],imageBounds[5]-imageBounds[4]);
	double xLength = cube->GetXLength();//imageBounds[1]-imageBounds[0];
	double yLength = cube->GetYLength();//imageBounds[3]-imageBounds[2];
	double zLength = cube->GetZLength();//imageBounds[5]-imageBounds[4];
	//cube->SetXLength(xLength);
	//cube->SetYLength(yLength);
	//cube->SetZLength(zLength);
//	qDebug()<<"xlength = "<<xLength<<" ylength = "<<yLength<<" zlength = "<<zLength;
	Point3D *point=new Point3D(xLength,yLength,zLength);
	Color3D *color=new Color3D(255,255,255);
	actor = createActors(point, color,-1);


	double *bounds;
	int slice=0;
	int *ix=new int[3];
	ix[0]=0;
	ix[1]=2;
	ix[2]=5;
	//--------------------------------------------------------------
	xyActor = vtkActor::New();
	bounds=mprbase->getXYViewer()->m_viewer->GetImageActor()->GetBounds();
	slice = mprbase->getXYViewer()->m_viewer->GetSlice();
	point=new Point3D(bounds[1]-bounds[0],bounds[3]-bounds[2],bounds[5]);
	xyPos=new Point3D(point);
	color=new Color3D(0,0,255);
	xyActor = createActors(point, color,mprbase->getXYViewer()->m_viewer->GetSliceOrientation());
	axialText = vtkCaptionActor2D::New();
	axialText = createCaptionActors(QString("A = %1").arg(slice+1).toStdString().c_str(), 10, 10, color, xyActor, ix);
	//--------------------------------------------------------------
	yzActor = vtkActor::New();
	bounds=mprbase->getYZViewer()->m_viewer->GetImageActor()->GetBounds();
	slice = mprbase->getYZViewer()->m_viewer->GetSlice();
	point=new Point3D(bounds[0],bounds[3]-bounds[2],bounds[5]-bounds[4]);
	yzPos=new Point3D(point);
	//point=new Point3D(0.0001, yLength, xLength);
	color=new Color3D(0,255,0);
	yzActor = createActors(point,color,mprbase->getYZViewer()->m_viewer->GetSliceOrientation());
	sagittalText = vtkCaptionActor2D::New();
	ix[1]=3;
	sagittalText = createCaptionActors(QString("S = %1").arg(slice+1).toStdString().c_str(), 5, 12,color, yzActor, ix);
	//--------------------------------------------------------------
	xzActor = vtkActor::New();
	bounds=mprbase->getXZViewer()->m_viewer->GetImageActor()->GetBounds();
	slice = mprbase->getXZViewer()->m_viewer->GetSlice();
	point=new Point3D(bounds[1]-bounds[0],bounds[2],bounds[5]-bounds[4]);
	xzPos=new Point3D(point);
	//point=new Point3D(xLength, yLength, 0.0001);
	color=new Color3D(255,0,0);
	xzActor = createActors(point, color,mprbase->getXZViewer()->m_viewer->GetSliceOrientation());
	coronalText = vtkCaptionActor2D::New();
	ix[0]=1;
	ix[2]=4;
	coronalText = createCaptionActors(QString("C = %1").arg(slice+1).toStdString().c_str(), -15, 15,color, xzActor,ix);

	vtkRenderer *renderer=mprbase->getRenderer3D();
	renderer->AddViewProp(actor);
	renderer->AddViewProp(xyActor);
	renderer->AddViewProp(xzActor);
	renderer->AddViewProp(yzActor);

	renderer->AddViewProp(axialText);
	renderer->AddViewProp(sagittalText);
	renderer->AddViewProp(coronalText);

	actor->Delete();
}
vtkCaptionActor2D* MPRCube::createCaptionActors(const char *label, int x, int y, Color3D *color, vtkActor *actor, int *index)
{
	vtkCaptionActor2D *labelText = vtkCaptionActor2D::New();
	labelText->SetCaption(label);
	labelText->SetPosition(x, y);
	labelText->SetBorder(0);
	labelText->GetProperty()->SetColor(color->getColor3D());
	double *bounds = new double[6];
	bounds = actor->GetBounds();
	labelText->SetAttachmentPoint(bounds[index[0]], bounds[index[1]], bounds[index[2]]);
	labelText->SetLeader(0);
	labelText->GetCaptionTextProperty()->SetColor(color->getColor3D());
	labelText->GetCaptionTextProperty()->SetFontFamilyToArial();
	labelText->GetCaptionTextProperty()->ShadowOff();
	labelText->GetCaptionTextProperty()->SetFontSize(10);
	labelText->GetCaptionTextProperty()->SetItalic(0);
	labelText->GetCaptionTextProperty()->SetBold(0);
	return labelText;
}
vtkActor* MPRCube::createActors(Point3D *point, Color3D *color,int orientation)
{
	vtkCubeSource *cube = vtkCubeSource::New();
	//cube->SetBounds(point->getAll());
	cube->SetCenter(0,0,0);
	switch(orientation)
	{
	case SAGITTAL:
		cube->SetXLength(0.0001);
		cube->SetYLength(point->getY());
		cube->SetZLength(point->getZ());
		point->setX(point->getX()*2);
		break;
	case CORONAL:
		cube->SetXLength(point->getX());
		cube->SetYLength(0.0001);
		cube->SetZLength(point->getZ());
		point->setY(point->getY()*2);
		break;
	case AXIAL:
		cube->SetXLength(point->getX());
		cube->SetYLength(point->getY());
		cube->SetZLength(0.0001);
		point->setZ(point->getZ()*2);
		break;
	default:
		cube->SetXLength(point->getX());
		cube->SetYLength(point->getY());
		cube->SetZLength(point->getZ());

		break;
	}
	
	vtkPolyDataMapper *tempMapper = vtkPolyDataMapper::New();
	tempMapper->SetInput(cube->GetOutput());
	vtkActor *actor = vtkActor::New();
	actor->GetProperty()->SetAmbient(1);
	actor->GetProperty()->SetDiffuse(1);
	actor->GetProperty()->SetSpecular(1);
	actor->GetProperty()->SetSpecularPower(5.0);
	actor->SetMapper(tempMapper);
	actor->GetProperty()->SetRepresentationToWireframe();
	actor->GetProperty()->BackfaceCullingOff();
	actor->GetProperty()->SetLineWidth(2.0);
	actor->GetProperty()->EdgeVisibilityOn();
	actor->GetProperty()->SetColor(color->getColor3D());
	actor->SetPosition(point->getAll());
	//actor->SetOrigin(point->getX()/2,point->getY()/2,point->getZ()/2);
	cube->Delete();
	tempMapper->Delete();
	return actor;
}
void MPRCube::setNewPositions(int slice, int orientation)
{
	double *pos = new double[3];
	double *posText = new double[6];
	double *bounds;
	Point3D *point;
	switch(orientation)
	{
	case SAGITTAL:
		bounds = mprbase->getYZViewer()->m_viewer->GetImageActor()->GetBounds();
		point=new Point3D(this->yzActor->GetPosition());
		point->setX(point->getX()-(yzPos->getX()-bounds[1]));
		this->yzActor->SetPosition(point->getAll()/*bounds[1],bounds[3]-bounds[2],bounds[5]-bounds[4]*//*pos[0] + (1.0 / (double) yzRange * slice * -1), pos[1], pos[2]*/);
		posText=this->yzActor->GetBounds();
		this->sagittalText->SetAttachmentPoint(posText[0], posText[3], posText[5]);
		this->sagittalText->SetCaption(QString("S = %1").arg(slice+1).toStdString().c_str());
		yzPos=new Point3D(bounds[1],bounds[3]-bounds[2],bounds[5]-bounds[4]);
		break;
	case CORONAL:
		bounds = mprbase->getXZViewer()->m_viewer->GetImageActor()->GetBounds();
		point=new Point3D(this->xzActor->GetPosition());
		point->setY(point->getY()-(xzPos->getY()-bounds[3]));
		this->xzActor->SetPosition(point->getAll()/*bounds[1]-bounds[0],bounds[3],bounds[5]-bounds[4]*//*pos[0], pos[1], pos[2] + (1.0 / (double) xzRange * slice * -1)*/);
		posText=this->xzActor->GetBounds();
		this->coronalText->SetAttachmentPoint(posText[1], posText[3], posText[4]);
		this->coronalText->SetCaption(QString("C = %1").arg(slice+1).toStdString().c_str());
		xzPos=new Point3D(bounds[1]-bounds[0],bounds[3],bounds[5]-bounds[4]);
		break;
	case AXIAL:
		bounds = mprbase->getXYViewer()->m_viewer->GetImageActor()->GetBounds();
		point=new Point3D(this->xyActor->GetPosition());
		point->setZ(point->getZ()-(xyPos->getZ()-bounds[5]));
		this->xyActor->SetPosition(point->getAll()/*bounds[1]-bounds[0],bounds[3]-bounds[2],bounds[5]*//*pos[0], pos[1] + (1.0 / (double) xyRange * slice * -1), pos[2]*/);
		posText=this->xyActor->GetBounds();
		this->axialText->SetAttachmentPoint(posText[0], posText[2], posText[5]);
		this->axialText->SetCaption(QString("A = %1").arg(slice+1).toStdString().c_str());
		xyPos=new Point3D(bounds[1]-bounds[0],bounds[3]-bounds[2],bounds[5]);
		break;
	}
	//point->printSelf();
	//qDebug()<<"Axes Actor bounds = "<<bounds[0]<<", "<<bounds[1]<<", "<<bounds[2]<<", "<<bounds[3]<<", "<<bounds[4]<<", "<<bounds[5];
	//double *temp=actor->GetBounds();
	//qDebug()<<"Actor bounds = "<<temp[0]<<", "<<temp[1]<<", "<<temp[2]<<", "<<temp[3]<<", "<<temp[4]<<", "<<temp[5];
	this->mprbase->getRenderer3D()->ResetCameraClippingRange();
	this->mprbase->getRenderWindow3D()->Render();
}
