package Drawings
{
	import flash.display.BitmapData;
	import flash.geom.Matrix;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	import mx.core.UIComponent;
	import mx.core.UITextField;
	import mx.graphics.ImageSnapshot;
	
	/** CircleTool: This component is created by Cantek ÇETİN
	 *  CircleTool for measuring area inside the Circle. 
	 */
	
	public class CircleTool extends DrawingTool
	{
		private static var MINIMUM_WIDTH:Number = 2;
		private static var MINIMUM_HEIGHT:Number = 2;
		public var uit:UITextField = new UITextField();
		public var w:Number;
		public var h:Number; 
		
		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void 
		{			
			if(startX != endX && startY != endY)
			{
				
				if( !isNaN(endX) )
				{
					w = endX - startX;
				}
				else
				{
					w = unscaledWidth;
				}
				graphics.clear();
				graphics.lineStyle(1, textFormat.color as uint,1);
				graphics.beginFill(0xFFFFFF,.5);
				graphics.drawCircle(startX,startY,w);
				uit.autoSize =  TextFieldAutoSize.LEFT;
				uit.defaultTextFormat = textFormat;	
				/*
				if(zoomFactor < 0)
				{
					zoomFactor = Math.abs(1/zoomFactor);
				}
				else if(zoomFactor > 0)
				{
					zoomFactor = zoomFactor;
				}
				else
				{
					zoomFactor = 1;
				}
				*/
				var measure:int = 3.14 * (w/2) * verpixelSpacing * (w/2) * horpixelSpacing / (zoomFactor * zoomFactor);
				uit.text = measure.toFixed(3).toString();
				var textBitmapData:BitmapData =	ImageSnapshot.captureBitmapData(uit);
				var sizeMatrix:Matrix = new Matrix();
				var coef:Number =Math.min(uit.measuredWidth/textBitmapData.width,uit.measuredHeight/textBitmapData.height);
				sizeMatrix.a = coef;
				sizeMatrix.d = coef;
				textBitmapData = ImageSnapshot.captureBitmapData(uit,sizeMatrix);
				graphics.lineStyle(0, 0,0);
				var sm:Matrix = new Matrix();
				sm.tx = (startX+endX)/2;
				sm.ty = (startY+endY)/2;
				graphics.beginBitmapFill(textBitmapData,sm,false);
				graphics.drawRect((startX+endX)/2,(startY+endY)/2,uit.measuredWidth,uit.measuredHeight);
				graphics.endFill();
			}
		}
	}
}