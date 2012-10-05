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
	
	/** FreeHandTool: This component is created by Cantek ÇETİN
	 *  FreeHandTool for drawing freehand shapes. 
	 */
	public class FreeHandTool extends DrawingTool
	{	
		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void//,pixelSpacing:Number 
		{
			if(startX != endX && startY != endY)
			{
				
				graphics.lineStyle(1, textFormat.color as uint,1);
				graphics.moveTo(startX,startY);
				graphics.lineTo(endX,endY);
				startX = endX;
				startY = endY;
				graphics.endFill();
			}
		}
	}
}