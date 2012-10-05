package classes
{
	import flash.display.BitmapData;
	import flash.filters.BevelFilter;
	import flash.filters.BitmapFilterQuality;
	import flash.filters.BlurFilter;
	import flash.filters.ConvolutionFilter;
	import flash.filters.DropShadowFilter;
	import flash.filters.GlowFilter;
	import flash.geom.ColorTransform;
	import flash.geom.Matrix;
	
	import mx.collections.ArrayCollection;
	import mx.core.EdgeMetrics;
	
	import spark.components.Image;
	import spark.filters.ColorMatrixFilter;

	public class imageFilters
	{
		public function imageFilters()
		{
			this.color.brightness = 0;
			this.color.saturation = 0;
			this.color.hue = 0;
			this.color.contrast = 0;			
		}
		public var filtersArrayAll:Array = new Array();
		public var color:AdjustColor = new AdjustColor();
		
		public function invertImage():Array
		{
			var matrix:Array = new Array();
			matrix = matrix.concat([-1, 0, 0, 0, 255]); // red
			matrix = matrix.concat([0, -1, 0, 0, 255]); // green
			matrix = matrix.concat([0, 0, -1, 0, 255]); // blue
			matrix = matrix.concat([0, 0, 0, 1, 0]); 	// alpha
			var cmf:ColorMatrixFilter = new ColorMatrixFilter(matrix);
			filtersArrayAll.push(cmf);
			return filtersArrayAll;
		}
		
		public function negativeImage():Array
		{
			var matrix:Array = new Array();
			matrix = matrix.concat([.33,.33,.33,0,0]); // red
			matrix = matrix.concat([.33,.33,.33,0,0]); // green
			matrix = matrix.concat([.33,.33,.33,0,0]); // blue
			matrix = matrix.concat([.33,.33,.33,1,0]); 	// alpha
			var cmf:ColorMatrixFilter = new ColorMatrixFilter(matrix);
			filtersArrayAll.push(cmf);
			return filtersArrayAll;
		}
		
		public function emboss(ratio:int):Array
		{			
			var emboss:ConvolutionFilter = new ConvolutionFilter(ratio, ratio, [-2, -1, 0, -1, 1, 1, 0, 1, 2]);
			emboss.divisor = 1;
			filtersArrayAll.push(emboss);
			return filtersArrayAll;
		}
		public function bevel(distance:int):Array
		{
			var bevel:BevelFilter = new BevelFilter();
			bevel.distance = distance;
			bevel.quality = BitmapFilterQuality.HIGH;
			bevel.strength = 120;
			bevel.highlightAlpha = 0.5;
			bevel.shadowAlpha = 0.5;
			filtersArrayAll.push(bevel);
			return filtersArrayAll;
		}
		public function glow():Array
		{
			var glow:GlowFilter = new GlowFilter();
			glow.inner = true;
			glow.strength = 120;
			glow.color = 0xfffc76;
			glow.alpha = 0.5;
			glow.quality = BitmapFilterQuality.HIGH;
			//glow.knockout = true;
			filtersArrayAll.push(glow);
			return filtersArrayAll;
		}
		
		public function dropShadow(distance:int):Array
		{
			var dropShadow:DropShadowFilter = new DropShadowFilter();
			dropShadow.inner = true;
			dropShadow.distance = distance;
			dropShadow.strength = 120;
			dropShadow.alpha = 0.5;
			dropShadow.quality=BitmapFilterQuality.HIGH;
			filtersArrayAll.push(dropShadow);
			return filtersArrayAll;
		}
		public function edgeDetection(ratio:int):Array
		{
			emboss(ratio);
			var edge:ConvolutionFilter = new ConvolutionFilter(ratio, ratio, [0, -1, 0, -1, 4, -1, 0, -1, 0]);
			filtersArrayAll.push(edge);
			return filtersArrayAll;
		}
		public function sharpen(ratio:int):Array
		{
			var sharpen:ConvolutionFilter = new ConvolutionFilter(ratio, ratio, [0, -1, 0, -1, 5, -1, 0, -1, 0]);
			sharpen.divisor = 1;
			filtersArrayAll.push(sharpen);
			return filtersArrayAll;
		}
		public function blur():Array
		{
			var blur:BlurFilter = new BlurFilter();
			filtersArrayAll.push(blur);
			return filtersArrayAll;
		}
		public function takeLastBack():Array
		{
			filtersArrayAll.pop();
			return filtersArrayAll;
		}
		
		public function resetFiltersArray():void
		{
			filtersArrayAll = null;
			filtersArrayAll = new Array();

		}
		
		public function resetColor(image:Image):void
		{
			var modifiedCt:ColorTransform = new  ColorTransform();
			var color:uint = 0x000000;
			var red:Number = ( color >> 16 ) & 0xff;
			var green:Number = ( color >> 8 ) & 0xff;
			var blue:Number = ( color & 0xff );
			modifiedCt.redOffset = red;
			modifiedCt.blueOffset = green;
			modifiedCt.greenOffset = blue;
			image.transform.colorTransform = modifiedCt;
		}
		
		public function setColor(color:uint,image:Image):void
		{
			var modifiedCt:ColorTransform = new  ColorTransform();
			var red:Number = ( color >> 16 ) & 0xff;
			var green:Number = ( color >> 8 ) & 0xff;
			var blue:Number = ( color & 0xff );
			modifiedCt.redOffset = red;
			modifiedCt.blueOffset = blue;
			modifiedCt.greenOffset = green;
			modifiedCt.alphaOffset = 1;
			image.transform.colorTransform = modifiedCt;
		}
		
		public var filter:ColorMatrixFilter = null;
		
		
		public function brightness(brightness:int,hue:int,sat:int,cont:int):Array
		{
			//color.brightness = 0;
			color.brightness = brightness;
			color.hue = hue;
			color.saturation = sat;
			color.contrast = cont;
			if(filter != null)
			{
				var tempCmF:ColorMatrixFilter = filtersArrayAll[filtersArrayAll.length-1] as ColorMatrixFilter;
				if(tempCmF != null)
				{
					filtersArrayAll.pop();
				}
			}
			filter = new ColorMatrixFilter(color.CalculateFinalFlatArray());
			filtersArrayAll.push(filter);
			return filtersArrayAll;
		}
	}
}