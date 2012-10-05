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
	/** AngleTool: This component is created by Cantek ÇETİN
	 *  AngleTool for measuring angle between two lines (three points) 
	 */
	public class AngleTool extends DrawingTool
	{
		public var firstPoint:Point = new Point();
		public var lastPoint:Point = new Point();
		public var interceptionPoint:Point = new Point();
		public var radianAdded:Boolean = false;
		public var uit:UITextField = new UITextField();
		public var textBitmapData:BitmapData;
		public var sizeMatrix:Matrix = new Matrix();
		public var sm:Matrix = new Matrix();
		
		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void//,pixelSpacing:Number 
		{
			graphics.clear();
			graphics.lineStyle(1, textFormat.color as uint,1);
			graphics.moveTo(startX,startY);
			graphics.lineTo(endX,endY);
			uit.defaultTextFormat = textFormat;
			if(radianAdded)
			{
				var lengthF2I:Number = Point.distance(firstPoint,interceptionPoint);
				var lengthI2L:Number = Point.distance(interceptionPoint,lastPoint);
				var lengthL2F:Number = Point.distance(lastPoint,firstPoint);
				var gamma:Number = Math.acos(((lengthF2I*lengthF2I)+(lengthI2L*lengthI2L)-(lengthL2F*lengthL2F))/(2*lengthF2I*lengthI2L));
				var measure:Number = gamma * 180/3.14;
				uit.text ="@ = " +  measure.toFixed(3).toString();
				
				/*
				var offsetWidth:Number = uit.width/2.0;
				var offsetHeight:Number =  uit.height/2.0;
				var radians:Number = (rotateAngle % 360) * (Math.PI / 180.0);
				var matrix:Matrix = new Matrix();
				matrix.translate(-offsetWidth, -offsetHeight);
				matrix.rotate(radians);
				matrix.translate(+offsetWidth, +offsetHeight);
				matrix.concat(uit.transform.matrix);
				uit.transform.matrix = matrix;

				if(verticalFlip)
				{
					var flipVerticalMatrix:Matrix = uit.transform.matrix;
					flipVerticalMatrix.scale(1,-1);
					flipVerticalMatrix.translate(0,height);
					uit.transform.matrix = flipVerticalMatrix;
				}
				if(horizontalFlip)
				{
					var flipHorizontalMatrix:Matrix = uit.transform.matrix;
					flipHorizontalMatrix.scale(-1,1);
					flipHorizontalMatrix.translate(width,0);
					uit.transform.matrix = flipHorizontalMatrix;
				}
				*/

				uit.autoSize =  TextFieldAutoSize.LEFT;
				uit.defaultTextFormat = textFormat;
				textBitmapData = ImageSnapshot.captureBitmapData(uit);
				var coef:Number = Math.min(uit.measuredWidth/textBitmapData.width,uit.measuredHeight/textBitmapData.height);
				sizeMatrix.a = coef;
				sizeMatrix.d = coef;
				textBitmapData = ImageSnapshot.captureBitmapData(uit,sizeMatrix);
				graphics.lineStyle(0, 0,0);
				sm.tx = (interceptionPoint.x);
				sm.ty = (interceptionPoint.y);
				graphics.beginBitmapFill(textBitmapData,sm,false);
				graphics.drawRect(interceptionPoint.x,interceptionPoint.y,uit.measuredWidth,uit.measuredHeight);
				graphics.endFill();
			}
		}
		
		public function setSecond():void
		{
			graphics.moveTo(startX,startY);
			graphics.lineTo(endX,endY);
		}
		
		public function setThird():void
		{
			radianAdded = true;
		}		
	}
}