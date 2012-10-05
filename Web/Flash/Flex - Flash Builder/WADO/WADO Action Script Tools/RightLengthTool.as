package Drawings
{
	import flash.display.BitmapData;
	import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	import flashx.textLayout.formats.Float;
	
	import mx.core.UIComponent;
	import mx.core.UITextField;
	import mx.graphics.ImageSnapshot;

	/** RightLengthTool: This component is created by Cantek ÇETİN
	 *  RightLengthTool for measuring an unchanged display length with changed properties
	 */

	public class RightLengthTool extends DrawingTool
	{        
		public var uit:UITextField = new UITextField();

		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void//,pixelSpacing:Number 
		{
			if(zoomFactor == 0)
				zoomFactor = 1;
			graphics.clear();
			if(clientHeight != 0 && clientWidth != 0)
			{
				graphics.lineStyle(1, textFormat.color as uint,1);
				graphics.moveTo(clientWidth - 40,clientHeight/2 + 30);
				graphics.lineTo(clientWidth - 40,clientHeight/2 - 30);
				var startP:Point = new Point(clientWidth - 10,clientHeight/2 + 10);
				var endP:Point = new Point(clientWidth - 10,clientHeight/2 - 10);
				var measure:Number = Point.distance(startP,endP);
				measure = measure * ((pixelSpacing / zoomFactor));
				uit.autoSize =  TextFieldAutoSize.LEFT;
				uit.defaultTextFormat = textFormat;
				uit.text = measure.toFixed(0).toString();
				var textBitmapData:BitmapData =	ImageSnapshot.captureBitmapData(uit);
				var sizeMatrix:Matrix = new Matrix();
				var coef:Number = Math.min(uit.measuredWidth/textBitmapData.width,uit.measuredHeight/textBitmapData.height);
				sizeMatrix.a = coef;
				sizeMatrix.d = coef;
				textBitmapData = ImageSnapshot.captureBitmapData(uit,sizeMatrix);
				graphics.lineStyle(0,0,0);
				var sm:Matrix = new Matrix();
				sm.tx = (clientWidth - 30);
				sm.ty = (clientHeight/2 + 40);
				graphics.beginBitmapFill(textBitmapData,sm,false);
				graphics.drawRect(clientWidth - 30,clientHeight/2 + 40,uit.measuredWidth,uit.measuredHeight);
				graphics.endFill();
			}
		}
	}
}