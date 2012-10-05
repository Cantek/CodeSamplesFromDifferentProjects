package Drawings
{
	import flash.text.TextFormat;
	
	import mx.core.UIComponent;
	/** DrawingTool: This component is created by Cantek ÇETİN
	 *  DrawingTool with extended properties for geometric and nongeometric shapes 
	 */
	public class DrawingTool extends UIComponent
	{
		public var startX:Number;
		public var startY:Number;
		public var endX:Number;
		public var endY:Number;
		public var pixelSpacing:Number;
		public var zoomFactor:Number;
		public var scaleXVal:Number;
		public var scaleYVal:Number;
		public var textFormat:TextFormat;
		public var rotateAngle:int;
		public var verticalFlip:Boolean;
		public var horizontalFlip:Boolean;
		public var clientWidth:Number;
		public var clientHeight:Number;
		public var horpixelSpacing:Number;
		public var verpixelSpacing:Number;
	}
}