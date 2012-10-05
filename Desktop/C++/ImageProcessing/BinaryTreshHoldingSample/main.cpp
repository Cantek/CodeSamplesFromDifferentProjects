#include "binarythresholding.h"
#include <QtGui/QApplication>

int main(int argc, char *argv[])
{
	QApplication a(argc, argv);
	BinaryThresholding w;
	w.show();
	return a.exec();
}
