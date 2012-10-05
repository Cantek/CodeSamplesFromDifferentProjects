#include "cubepanel.h"
#include "vtkActor.h"
#include "vtkRenderer.h"
#include "vtkRenderWindow.h"
#include "vtkAxesActor.h"
#include "vtkCaptionActor2D.h"
#include "vtkCubeSource.h"
#include "vtkMatrix4x4.h"
#include "vtkPolyDataMapper.h"


CubePanel::CubePanel(QWidget *parent, Qt::WFlags flags)
: QMainWindow(parent, flags)
{
	ui.setupUi(this);
	
	vtkRenderWindow *renWin = vtkRenderWindow::New();

	vtkRenderer *renderer = vtkRenderer::New();



	double xLength,  yLength,  zLength;
	vtkCaptionActor2D *axialText = vtkCaptionActor2D::New(); 
	vtkCaptionActor2D *sagittalText = vtkCaptionActor2D::New();
	vtkCaptionActor2D *coronalText = vtkCaptionActor2D::New();
	vtkAxesActor *axesUp = vtkAxesActor::New();
	vtkAxesActor *axesDown = vtkAxesActor::New();
	vtkCaptionActor2D *axesUp_x = vtkCaptionActor2D::New();
	vtkCaptionActor2D *axesUp_y = vtkCaptionActor2D::New();
	vtkCaptionActor2D *axesUp_z = vtkCaptionActor2D::New();
	vtkCaptionActor2D *axesDown_x = vtkCaptionActor2D::New();
	vtkCaptionActor2D *axesDown_y = vtkCaptionActor2D::New();
	vtkCaptionActor2D *axesDown_z = vtkCaptionActor2D::New();
	vtkActor *axialActor = vtkActor::New();
	vtkActor *sagittalActor = vtkActor::New();
	vtkActor *coronalActor = vtkActor::New();
	int axialRange;
	int sagittalRange;
	int coronalRange;
}

createVTKPipeline() {
	this.Lock();
	vtkCubeSource *cube = vtkCubeSource::New();
	vtkActor *actor = vtkActor::New();
	xLength = cube->GetXLength();
	yLength = cube->GetYLength();
	zLength = cube->GetZLength();
	actor = CreateActors(xLength, yLength, zLength, 1, 1, 1);
	axesUp_x = vtkCaptionActor2D::New();
	axesUp_y = vtkCaptionActor2D::New();
	axesUp_z = vtkCaptionActor2D::New();
	axesDown_x = vtkCaptionActor2D::New();
	axesDown_y = vtkCaptionActor2D::New();
	axesDown_z = vtkCaptionActor2D::New();
	axesUp = vtkAxesActor::New();
	axesUp = CreateAxesActor(1.0);
	axesDown = vtkAxesActor::New();
	axesDown = CreateAxesActor(-1.0);

	axialActor = vtkActor::New();
	axialActor = CreateActors(xLength, 0.0001, zLength, 0, 0, 1);
	axialText = vtkCaptionActor2D::New();
	axialText = CreateCaptionActors("A", 10, 10, 0, 0, 1, axialActor, 0, 2, 5);

	sagittalActor = vtkActor::New();
	sagittalActor = CreateActors(0.0001, yLength, xLength, 0, 1, 0);
	sagittalText = vtkCaptionActor2D::New();
	sagittalText = CreateCaptionActors("S", 5, 12, 0, 1, 0, sagittalActor, 0, 3, 5);

	coronalActor = vtkActor::New();
	coronalActor = CreateActors(xLength, yLength, 0.0001, 1, 0, 0);
	coronalText = vtkCaptionActor2D::New();
	coronalText = CreateCaptionActors("C", -15, 15, 1, 0, 0, coronalActor, 1, 3, 4);


	
	double[] data = new double[3];
	data = camera.GetPosition();
	this.camera.SetPosition(data[0] - 1, data[1] + 1.5, data[2] - 5);
	this.camera.Azimuth(45.0);
	this.renderer.AddActor(actor);
	this.renderer.AddActor(axialActor);
	this.renderer.AddActor(sagittalActor);
	this.renderer.AddActor(coronalActor);

	this.renderer.AddActor(axesUp);
	this.renderer.AddActor(axesDown);

	this.renderer.AddActor(axesUp_x);
	this.renderer.AddActor(axesDown_x);
	this.renderer.AddActor(axesUp_y);
	this.renderer.AddActor(axesDown_y);
	this.renderer.AddActor(axesUp_z);
	this.renderer.AddActor(axesDown_z);

	this.renderer.AddActor(axialText);
	this.renderer.AddActor(sagittalText);
	this.renderer.AddActor(coronalText);

	this.renderer.ResetCameraClippingRange();
	this.UnLock();
}


vtkCaptionActor2D CreateCaptionActors(String label, int x, int y, double r, double g, double b, vtkActor actor, int xIndex, int yIndex, int zIndex) {
	vtkCaptionActor2D labelText = new vtkCaptionActor2D();
	labelText.SetCaption(label);
	labelText.SetPosition(x, y);
	labelText.SetBorder(0);
	labelText.GetProperty().SetColor(r, g, b);
	double[] bounds = new double[6];
	bounds = actor.GetBounds();
	labelText.SetAttachmentPoint(bounds[xIndex], bounds[yIndex], bounds[zIndex]);
	labelText.SetLeader(0);
	labelText.GetCaptionTextProperty().SetColor(r, g, b);
	labelText.GetCaptionTextProperty().SetFontFamilyToArial();
	labelText.GetCaptionTextProperty().ShadowOff();
	//labelText.GetTextActor().ScaledTextOff();
	labelText.GetCaptionTextProperty().SetFontSize(20);
	labelText.GetCaptionTextProperty().SetItalic(0);
	labelText.GetCaptionTextProperty().SetBold(0);
	return labelText;
}

vtkActor CreateActors(double x, double y, double z, double r, double g, double b) {
	vtkCubeSource cube = new vtkCubeSource();
	cube.SetXLength(x);
	cube.SetYLength(y);
	cube.SetZLength(z);
	vtkPolyDataMapper tempMapper = new vtkPolyDataMapper();
	tempMapper.SetInput(cube.GetOutput());
	vtkActor actor = new vtkActor();
	actor.GetProperty().SetAmbient(1);
	actor.GetProperty().SetDiffuse(1);
	actor.GetProperty().SetSpecular(1);
	actor.GetProperty().SetSpecularPower(5.0);
	actor.SetMapper(tempMapper);
	actor.GetProperty().SetRepresentationToWireframe();
	actor.GetProperty().BackfaceCullingOff();
	actor.GetProperty().SetLineWidth(2.0);
	actor.GetProperty().EdgeVisibilityOn();
	actor.GetProperty().SetColor(r, g, b);
	cube.Delete();
	tempMapper.Delete();

	return actor;
}

vtkAxesActor CreateAxesActor(double whichway) {
	vtkAxesActor axes = new vtkAxesActor();
	double[] axesBounds = new double[6];
	axes.SetConeRadius(0.3);
	double axesTransform[] = {whichway, 0, 0, 1.5,
		0, whichway, 0, 2.0,
		0, 0, whichway, -2.5,
		0, 0, 0, 3
	};
	vtkMatrix4x4 matrix = new vtkMatrix4x4();
	matrix.DeepCopy(axesTransform);
	axes.SetUserMatrix(matrix);
	axesBounds = axes.GetBounds();
	axes.AxisLabelsOff();
	axes.GetXAxisShaftProperty().SetColor(1, 0, 0);
	axes.GetYAxisShaftProperty().SetColor(0, 1, 0);
	axes.GetZAxisShaftProperty().SetColor(0, 0, 1);
	axes.GetXAxisTipProperty().SetColor(1, 0, 0);
	axes.GetYAxisTipProperty().SetColor(0, 1, 0);
	axes.GetZAxisTipProperty().SetColor(0, 0, 1);
	axes.GetXAxisTipProperty().SetColor(1, 0, 0);
	axes.GetYAxisTipProperty().SetColor(0, 1, 0);
	if (whichway > 0) {
		axesUp_x = CreateAxesCaptionActors(axesUp_x, ("R"), -15, 0, 1, 0, 0, (axesTransform[3] / axesTransform[15]) + (axesBounds[1] / axesTransform[15]), axesTransform[7] / axesTransform[15], axesTransform[11] / axesTransform[15]);
		axesUp_y = CreateAxesCaptionActors(axesUp_y, ("H"), -10, 5, 0, 0, 1, axesTransform[3] / axesTransform[15], axesTransform[7] / axesTransform[15] + axesBounds[3] / axesTransform[15], axesTransform[11] / axesTransform[15]);
		axesUp_z = CreateAxesCaptionActors(axesUp_z, ("A"), -10, 0, 0, 1, 0, axesTransform[3] / axesTransform[15], axesTransform[7] / axesTransform[15], axesTransform[11] / axesTransform[15] + axesBounds[4] / axesTransform[15]);
	} else {
		axesDown_x = CreateAxesCaptionActors(axesDown_x, ("L"), -5, -5, 1, 0, 0, (axesTransform[3] / axesTransform[15]) - (axesBounds[1] / axesTransform[15]), axesTransform[7] / axesTransform[15], axesTransform[11] / axesTransform[15]);
		axesDown_y = CreateAxesCaptionActors(axesDown_y, ("F"), -5, -5, 0, 0, 1, axesTransform[3] / axesTransform[15], axesTransform[7] / axesTransform[15] - axesBounds[3] / axesTransform[15], axesTransform[11] / axesTransform[15]);
		axesDown_z = CreateAxesCaptionActors(axesDown_z, ("P"), 0, 0, 0, 1, 0, axesTransform[3] / axesTransform[15], axesTransform[7] / axesTransform[15], axesTransform[11] / axesTransform[15] - axesBounds[4] / axesTransform[15]);
	}
	matrix.Delete();
	return axes;
}

vtkCaptionActor2D CreateAxesCaptionActors(vtkCaptionActor2D labelText, String label, int x, int y, double r, double g, double b, double xCoor, double yCoor, double zCoor) {
	labelText.SetCaption(label);
	labelText.SetPosition(x, y);
	labelText.SetBorder(0);
	labelText.GetProperty().SetColor(r, g, b);
	labelText.SetAttachmentPoint(xCoor, yCoor, zCoor);
	labelText.GetCaptionTextProperty().SetColor(r, g, b);
	labelText.GetCaptionTextProperty().SetFontFamilyToArial();
	labelText.GetCaptionTextProperty().ShadowOff();
	//        labelText.GetTextActor().ScaledTextOn();
	labelText.GetCaptionTextProperty().SetFontSize(15);
	labelText.GetCaptionTextProperty().SetItalic(0);
	labelText.GetCaptionTextProperty().SetBold(0);
	labelText.SetLeader(0);
	return labelText;
}


public void setAxialRange(int axialRange) {
	this.axialRange = axialRange;
}

public void setSagittalRange(int sagittalRange) {
	this.sagittalRange = sagittalRange;
}

public void setCoronalRange(int coronalRange) {
	this.coronalRange = coronalRange;
}


public void setNewPositions(int motion, int whichOne) {
	double pos[] = new double[3];
	double posText[] = new double[6];
	switch (whichOne) {
			case Constants.AXIAL: {
				//                this.sagittalActor.GetProperty().SetRepresentationToWireframe();
				//                this.coronalActor.GetProperty().SetRepresentationToWireframe();
				//                this.axialActor.GetProperty().SetRepresentationToSurface();
				pos = this.axialActor.GetPosition();
				this.axialActor.SetPosition(pos[0], pos[1] + (1.0 / (double) axialRange * motion * -1), pos[2]);
				posText = this.axialActor.GetBounds();
				this.axialText.SetAttachmentPoint(posText[0], posText[2], posText[5]);
				break;
								  }
			case Constants.SAGITTAL: {
				//                this.axialActor.GetProperty().SetRepresentationToWireframe();
				//                this.coronalActor.GetProperty().SetRepresentationToWireframe();
				//                this.sagittalActor.GetProperty().SetRepresentationToSurface();
				pos = this.sagittalActor.GetPosition();
				this.sagittalActor.SetPosition(pos[0] + (1.0 / (double) sagittalRange * motion * -1), pos[1], pos[2]);
				posText = this.sagittalActor.GetBounds();
				this.sagittalText.SetAttachmentPoint(posText[0], posText[3], posText[5]);
				break;
									 }
			case Constants.CORONAL: {
				//                this.sagittalActor.GetProperty().SetRepresentationToWireframe();
				//                this.axialActor.GetProperty().SetRepresentationToWireframe();
				//                this.coronalActor.GetProperty().SetRepresentationToSurface();
				pos = this.coronalActor.GetPosition();
				this.coronalActor.SetPosition(pos[0], pos[1], pos[2] + (1.0 / (double) coronalRange * motion * -1));
				posText = this.coronalActor.GetBounds();
				this.coronalText.SetAttachmentPoint(posText[1], posText[3], posText[4]);
				break;
									}
	}
	this.Render();
}

CubePanel::~CubePanel()
{

}
