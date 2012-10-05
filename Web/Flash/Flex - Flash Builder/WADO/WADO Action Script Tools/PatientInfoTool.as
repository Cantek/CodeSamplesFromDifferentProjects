package Drawings
{
	import flash.display.BitmapData;
	import flash.geom.ColorTransform;
	import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	import flashx.textLayout.formats.Float;
	
	import mx.collections.ArrayCollection;
	import mx.core.UIComponent;
	import mx.core.UITextField;
	import mx.graphics.ImageSnapshot;

	/** PatientInfoTool: This component is created by Cantek ÇETİN
	 *  PatientInfoTool for adding patient informations at desired position on display obejct. 
	 */

	public class PatientInfoTool extends DrawingTool
	{
		public var topLeft:ArrayCollection;
		public var topRight:ArrayCollection;
		public var bottomLeft:ArrayCollection;
		public var bottomRight:ArrayCollection;
		
		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void//,pixelSpacing:Number 
		{
			graphics.clear();
			setTopLeft();
			setTopRight();
			setBottomLeft();
			setBottomRight();
		}
		
		private function setTopLeft():void
		{
			var uit:UITextField = new UITextField();
			uit.text ="";
			uit.autoSize = TextFieldAutoSize.LEFT;
			uit.explicitWidth = clientWidth/2;
			//uit.background = true;
			uit.defaultTextFormat = textFormat;
			if(topLeft != null)
			{
				for(var i:int = 0; i < topLeft.length; i++)
				{
					uit.text += topLeft[i].toString() + "\n";
				}
				
				var textBitmapData:BitmapData =	ImageSnapshot.captureBitmapData(uit);
				var sizeMatrix:Matrix = new Matrix();
				var coef:Number = Math.min(uit.measuredWidth/textBitmapData.width,uit.measuredHeight/textBitmapData.height);
				sizeMatrix.a = coef;
				sizeMatrix.d = coef;
				textBitmapData = ImageSnapshot.captureBitmapData(uit,sizeMatrix);
				graphics.lineStyle(0, 0,0);
				var sm:Matrix = new Matrix();
				sm.tx = 0;
				sm.ty = 0;
				graphics.beginBitmapFill(textBitmapData,sm,false);
				graphics.drawRect(0,0,uit.measuredWidth,uit.measuredHeight);
				graphics.endFill();
			}
		}
		private function setTopRight():void
		{
			var uit:UITextField = new UITextField();
			uit.text ="";
			uit.autoSize = TextFieldAutoSize.LEFT;
			uit.explicitWidth = clientWidth/2;
			//uit.background = true;
			uit.defaultTextFormat = textFormat;
			if(topRight != null)
			{
				for(var i:int = 0; i < topRight.length; i++)
				{
					uit.text += topRight[i].toString()+"\n";
				}
				
				uit.autoSize =  TextFieldAutoSize.LEFT;
				var textBitmapData:BitmapData =	ImageSnapshot.captureBitmapData(uit);
				var sizeMatrix:Matrix = new Matrix();
				var coef:Number = Math.min(uit.measuredWidth/textBitmapData.width,uit.measuredHeight/textBitmapData.height);
				sizeMatrix.a = coef;
				sizeMatrix.d = coef;
				textBitmapData = ImageSnapshot.captureBitmapData(uit,sizeMatrix);
				graphics.lineStyle(0, 0,0);
				var sm:Matrix = new Matrix();
				sm.tx = clientWidth - uit.measuredWidth ;
				sm.ty = 0;
				graphics.beginBitmapFill(textBitmapData,sm,false);			
				graphics.drawRect(clientWidth - uit.measuredWidth ,0,uit.measuredWidth,uit.measuredHeight);
				graphics.endFill();
			}
		}
		private function setBottomLeft():void
		{
			var uit:UITextField = new UITextField();
			uit.text ="";
			uit.autoSize = TextFieldAutoSize.LEFT;
			uit.explicitWidth = clientWidth/2;
			//uit.background = true; 
			uit.defaultTextFormat = textFormat;
			if(bottomLeft != null)
			{
				for(var i:int = 0; i < bottomLeft.length; i++)
				{
					uit.text += bottomLeft[i].toString()+"\n";
				}
				
				uit.autoSize =  TextFieldAutoSize.LEFT;
				var textBitmapData:BitmapData =	ImageSnapshot.captureBitmapData(uit);
				var sizeMatrix:Matrix = new Matrix();
				var coef:Number = Math.min(uit.measuredWidth/textBitmapData.width,uit.measuredHeight/textBitmapData.height);
				sizeMatrix.a = coef;
				sizeMatrix.d = coef;
				textBitmapData = ImageSnapshot.captureBitmapData(uit,sizeMatrix);
				graphics.lineStyle(0, 0,0);
				var sm:Matrix = new Matrix();
				sm.tx = 0 ;
				sm.ty = clientHeight - uit.measuredHeight;
				graphics.beginBitmapFill(textBitmapData,sm,false);			
				graphics.drawRect(0 ,clientHeight - uit.measuredHeight,uit.measuredWidth,uit.measuredHeight);
				graphics.endFill();
			}
		}
		private function setBottomRight():void
		{
			var uit:UITextField = new UITextField();
			uit.text ="";
			uit.autoSize = TextFieldAutoSize.LEFT;
			uit.explicitWidth = clientWidth/2;
			//uit.background = true;
			uit.defaultTextFormat = textFormat;
			if(bottomRight != null)
			{
				for(var i:int = 0; i < bottomRight.length; i++)
				{
					uit.text += bottomRight[i].toString()+"\n";
				}
				uit.autoSize =  TextFieldAutoSize.LEFT;
				var textBitmapData:BitmapData =	ImageSnapshot.captureBitmapData(uit);
				var sizeMatrix:Matrix = new Matrix();
				var coef:Number = Math.min(uit.measuredWidth/textBitmapData.width,uit.measuredHeight/textBitmapData.height);
				sizeMatrix.a = coef;
				sizeMatrix.d = coef;
				textBitmapData = ImageSnapshot.captureBitmapData(uit,sizeMatrix);
				graphics.lineStyle(0, 0,0);
				var sm:Matrix = new Matrix();
				sm.tx = clientWidth - uit.measuredWidth;
				sm.ty = clientHeight - uit.measuredHeight;
				graphics.beginBitmapFill(textBitmapData,sm,false);			
				graphics.drawRect(clientWidth - uit.measuredWidth,clientHeight - uit.measuredHeight,uit.measuredWidth,uit.measuredHeight);
				graphics.endFill();
			}
		}
	}
}