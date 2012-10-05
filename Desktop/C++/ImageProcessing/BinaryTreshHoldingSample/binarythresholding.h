#ifndef BINARYTHRESHOLDING_H
#define BINARYTHRESHOLDING_H

#include <QtGui/QMainWindow>
#include "ui_binarythresholding.h"

class BinaryThresholding : public QMainWindow
{
	Q_OBJECT

public:
	BinaryThresholding(QWidget *parent = 0, Qt::WFlags flags = 0);
	~BinaryThresholding();

private:
	Ui::BinaryThresholdingClass ui;
};

#endif // BINARYTHRESHOLDING_H
