package Drawings
{
	import flash.display.BitmapData;
	import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	import flashx.textLayout.formats.Float;
	
	import mx.containers.Canvas;
	import mx.core.UIComponent;
	import mx.core.UITextField;
	import mx.graphics.ImageSnapshot;
	
	/** LineTool: This component is created by Cantek ÇETİN
	 *  LineTool for measuring length. 
	 */
	
	public class LineTool extends DrawingTool
	{        
		public var uit:UITextField = new UITextField();
		//public var parentObj:Canvas = this.parent.parent as Canvas;
		//uit.parent = parentObj;
		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void//,pixelSpacing:Number 
		{
			var parentObj:Canvas = this.parent.parent as Canvas;
			if(startX != endX && startY != endY)
			{
				
				graphics.clear();
				graphics.lineStyle(1, textFormat.color as uint,1);
				var tpoint:Point;
				var rpoint:Point;
				
				var tpointNew:Point;
				var rpointNew:Point;
				
				var startXNew:Number = startX;
				var startYNew:Number = startY;
				var endXNew:Number = endX;
				var endYNew:Number = endY;
				
				var startX2:Number = startX;
				var startY2:Number = startY;
				var endX2:Number = endX;
				var endY2:Number = endY;
				
				var xComP1:Point = new Point(startX,startY);
				var xComP2:Point = new Point(endX,startY);
				
				var yComP1:Point = new Point(endX,startY);
				var yComP2:Point = new Point(endX,endY);
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
				var measureXComp:Number = Point.distance(xComP1,xComP2) * horpixelSpacing/zoomFactor;
				var measureYComp:Number = Point.distance(yComP1,yComP2) * verpixelSpacing/zoomFactor;
				
				var measure:Number = Math.sqrt(Math.pow(measureXComp,2)+Math.pow(measureYComp,2));
				
				tpoint = new Point(startX,startY);
				rpoint = new Point(endX,endY);			
				
				graphics.moveTo(tpoint.x,tpoint.y);
				graphics.lineTo(rpoint.x,rpoint.y);
				
				tpointNew = new Point(startXNew,startYNew);
				rpointNew = new Point(endXNew,endYNew);
				//measure = measure * ((pixelSpacing / zoomFactor));
				uit.autoSize =  TextFieldAutoSize.LEFT;
				uit.defaultTextFormat = textFormat;
				uit.text = measure.toFixed(3).toString();			
				var textBitmapData:BitmapData =	ImageSnapshot.captureBitmapData(uit);
				var sizeMatrix:Matrix = new Matrix();
				var coef:Number = Math.min(uit.measuredWidth/textBitmapData.width,uit.measuredHeight/textBitmapData.height);
				sizeMatrix.a = coef;
				sizeMatrix.d = coef;
				textBitmapData = ImageSnapshot.captureBitmapData(uit,sizeMatrix);
				graphics.lineStyle(1,1,1);
				var sm:Matrix = new Matrix();
				sm.tx = (tpointNew.x+rpointNew.x)/2;
				sm.ty = (tpointNew.y+rpointNew.y)/2;
				
				graphics.beginBitmapFill(textBitmapData,sm,false);
				graphics.drawRect((tpointNew.x+rpointNew.x)/2,(tpointNew.y+rpointNew.y)/2,uit.measuredWidth,uit.measuredHeight);
				graphics.endFill();
				//parentObj.graphics.endFill();
			}
		}
	}
}