package Drawings
{
	import flash.display.BitmapData;
	import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	import flashx.textLayout.formats.Float;
	
	import mx.controls.TextArea;
	import mx.core.UIComponent;
	import mx.core.UITextField;
	import mx.graphics.ImageSnapshot;
	/** annotationItem: This component is created by Cantek ÇETİN
	 *  annotationItem for adding annotations on display objects with 
	 * changable line and text properties 
	 */
	public class annotationItem extends DrawingTool
	{
		public var ta:TextArea = new TextArea();
		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void//,pixelSpacing:Number 
		{
			if(startX != endX && startY != endY)
			{
				graphics.clear();
				graphics.lineStyle(1, textFormat.color as uint,1);
				graphics.moveTo(startX,startY);
				graphics.lineTo(endX,endY);
			}
		}
		
		public function addTa():void
		{
			parent.addChild(ta);
			ta.move(endX,endY);
			ta.focusEnabled = true;
			ta.setFocus();
		}
	}
}