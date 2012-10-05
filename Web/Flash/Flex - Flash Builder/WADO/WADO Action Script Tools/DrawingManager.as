package Drawings
{
	import Drawings.DrawingTool;
	
	import flash.events.MouseEvent;
	import flash.text.TextFormat;
	/** DrawingManager: This component is created by Cantek ÇETİN
	 *  DrawingManager for creating run time drawing tools 
	 */		
	public class DrawingManager
	{
		public function DrawingManager()
		{
		}
		
		public static function createTool(className:Class, event:MouseEvent,horpixelSpacing:Number,verpixelSpacing:Number,zoomFactorNew:Number,textFormat:TextFormat,rotateAngle:Number,verticalFlip:Boolean,horizontalFlip:Boolean,clientWidth:Number,clientHeight:Number):DrawingTool
		{			
			var newClass:DrawingTool = new className();

			var startXNew:Number = event.localX;
			var startYNew:Number = event.localY;
			var endXNew:Number = event.localX;
			var endYNew:Number = event.localY;
			var startX:Number = event.localX;
			var startY:Number = event.localY;
			var endX:Number = event.localX;
			var endY:Number = event.localY;
			/*
			if((rotateAngle % 360) == 0)
			{
			}
			if((rotateAngle % 360) == 90 || (rotateAngle % 360) == -90)
			{
				startX = clientWidth - startYNew;			
				startY = startXNew;
				endX = clientWidth - endYNew;
				endY = endXNew;
				startYNew = startY;
				startXNew = startX;
				endYNew = endY;
				endXNew = endX;
			}
			if((rotateAngle % 360) == 180 || (rotateAngle % 360) == -180)
			{
				startX = clientWidth - startXNew;
				startY = clientHeight - startYNew;
				endX = clientWidth - endXNew;
				endY = clientHeight - endYNew;
				startYNew = startY;
				startXNew = startX;
				endYNew = endY;
				endXNew = endX;
			}
			if((rotateAngle % 360) == 270 || (rotateAngle % 360) == -270)
			{
				startX = startYNew;
				startY = clientHeight - startX;
				endX = endY;
				endY = clientHeight - endX;
				startYNew = startY;
				startXNew = startX;
				endYNew = endY;
				endXNew = endX;
			}
			if(verticalFlip)
			{
				startX = startXNew;
				startY = clientHeight - startYNew;
				endX = endXNew;
				endY = clientHeight - endYNew;
				startYNew = startY;
				startXNew = startX;
				endYNew = endY;
				endXNew = endX;
			}
			if(horizontalFlip)
			{
				startX = clientWidth - startXNew;
				startY = startYNew;
				endX = clientWidth - endXNew;
				endY = endYNew;
				startYNew = startY;
				startXNew = startX;
				endYNew = endY;
				endXNew = endX;				
			}
			*/
			newClass.startX = startXNew;
			newClass.startY = startYNew;
			newClass.endX = endXNew;
			newClass.endY = endYNew;
			newClass.horpixelSpacing = horpixelSpacing;
			newClass.verpixelSpacing = verpixelSpacing;
			newClass.zoomFactor = zoomFactorNew;
			newClass.textFormat = textFormat;
			newClass.rotateAngle = rotateAngle;
			newClass.verticalFlip = verticalFlip;
			newClass.horizontalFlip = horizontalFlip;
			newClass.invalidateDisplayList();
			return newClass;
		}		
	}
}