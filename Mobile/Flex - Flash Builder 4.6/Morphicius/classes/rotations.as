package classes
{
	import flash.display.BitmapData;
	import flash.geom.Matrix;
	
	import org.osmf.layout.ScaleMode;
	
	import spark.components.Image;
	import spark.components.VideoDisplay;

	public class rotations
	{
		public function rotations()
		{
		}
		
		public function turnLeftFunction(image:Image):void
		{
			rotationAngle -= 90;
			var offsetWidth:Number = image.width/2.0;
			var offsetHeight:Number =  image.height/2.0;
			var radians:Number = -90 * (Math.PI / 180.0);
			var tempHeight:Number =  image.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(image.transform.matrix );
			image.transform.matrix = matrix;	
			//image.scaleMode = ScaleMode.LETTERBOX;
		}
		
		public function turnRightFunction(image:Image):void
		{
			rotationAngle += 90;
			var offsetWidth:Number = image.width/2.0;
			var offsetHeight:Number =  image.height/2.0;
			var radians:Number = 90 * (Math.PI / 180.0);
			var tempHeight:Number =  image.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(image.transform.matrix );
			image.transform.matrix = matrix;
			//image.scaleMode = ScaleMode.LETTERBOX;
		}

		public var matrixReal:Matrix = null;
		public var matrixCompass:Matrix = null;
		
		public function turnLeftRealFunction(image:Image):void
		{
			var offsetWidth:Number = image.parent.width/2.0;
			var offsetHeight:Number =  image.parent.height/2.0;
			var radians:Number = -90 * (Math.PI / 180.0);
			var tempHeight:Number =  image.parent.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(matrixReal);
			matrixReal = matrix;			
			image.transform.matrix = matrix;	
			//image.scaleMode = ScaleMode.LETTERBOX;
		}
		
		public function turnRightRealFunction(image:Image):void
		{
			var offsetWidth:Number = image.parent.width/2.0;
			var offsetHeight:Number =  image.parent.height/2.0;
			var radians:Number = 90 * (Math.PI / 180.0);
			var tempHeight:Number =  image.parent.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(matrixReal);
			matrixReal = matrix;
			image.transform.matrix = matrix;
			//image.scaleMode = ScaleMode.LETTERBOX;
		}

		
		public function turnRightFunctionForVideo(vd:VideoDisplay):void
		{
			//rotationAngle += 90;
			var offsetWidth:Number = vd.width/2.0;
			var offsetHeight:Number =  vd.height/2.0;
			var radians:Number = 90 * (Math.PI / 180.0);
			var tempHeight:Number =  vd.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(vd.transform.matrix );
			vd.transform.matrix = matrix;
			//vd.scaleMode = ScaleMode.LETTERBOX;
		}
		
		public function turnLeftFunctionForVideo(vd:VideoDisplay):void
		{
			var offsetWidth:Number = vd.width/2.0;
			var offsetHeight:Number =  vd.height/2.0;
			var radians:Number = -90 * (Math.PI / 180.0);
			var tempHeight:Number =  vd.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(vd.transform.matrix );
			vd.transform.matrix = matrix;
			//vd.scaleMode = ScaleMode.LETTERBOX;
		}
		
		public function flipHorizontalFunction(image:Image):void
		{
			var flipHorizontalMatrix:Matrix = image.transform.matrix;
			flipHorizontalMatrix.scale(-1,1);
			flipHorizontalMatrix.translate(image.width,0);
			image.transform.matrix = flipHorizontalMatrix;
			image.scaleMode = ScaleMode.LETTERBOX;
		}
		
		public function flipVerticalFunction(image:Image):void
		{
			var flipVerticalMatrix:Matrix = image.transform.matrix;
			flipVerticalMatrix.scale(1,-1);
			flipVerticalMatrix.translate(0,image.height);
			image.transform.matrix = flipVerticalMatrix;
			image.scaleMode = ScaleMode.LETTERBOX;
		}

		public function flipHorizontalRealFunction(image:Image):void
		{
			var flipHorizontalMatrix:Matrix = matrixReal;
			flipHorizontalMatrix.scale(-1,1);
			flipHorizontalMatrix.translate(image.parent.width,0);
			matrixReal = flipHorizontalMatrix;
			image.transform.matrix = flipHorizontalMatrix;
			image.scaleMode = ScaleMode.LETTERBOX;
		}
		
		public function flipVerticalRealFunction(image:Image):void
		{
			var flipVerticalMatrix:Matrix = matrixReal;
			flipVerticalMatrix.scale(1,-1);
			flipVerticalMatrix.translate(0,image.parent.height);
			matrixReal = flipVerticalMatrix;
			image.transform.matrix = flipVerticalMatrix;
			image.scaleMode = ScaleMode.LETTERBOX;
		}

		
		public function resetRotationsAndFlips(rotation:int,flipV:int,flipH:int,image:Image):void
		{
			var offsetWidth:Number = image.width/2.0;
			var offsetHeight:Number =  image.height/2.0;
			var radians:Number = -1* rotationAngle * (Math.PI / 180.0);
			var tempHeight:Number =  image.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(image.transform.matrix );
			image.transform.matrix = matrix;
			image.scaleMode = ScaleMode.LETTERBOX;
			
			if(flipV%2 != 0 && flipV)
				flipVerticalFunction(image);
			if(flipH%2 != 0)
				flipHorizontalFunction(image);
			
			//rotationAngle = 0;
		}

		public function resetRotationsAndFlipsReal(rotation:int,flipV:int,flipH:int,image:Image):void
		{
			var offsetWidth:Number = image.parent.width/2.0;
			var offsetHeight:Number =  image.parent.height/2.0;
			var radians:Number = -1*rotationAngle * (Math.PI / 180.0);
			var tempHeight:Number =  image.parent.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(matrixReal);
			matrixReal = matrix;
			image.transform.matrix = matrix;
			image.scaleMode = ScaleMode.LETTERBOX;
			
			if(flipV%2 != 0 && flipV)
				flipVerticalRealFunction(image);
			if(flipH%2 != 0)
				flipHorizontalRealFunction(image);
		}


		public var rotationAngle:int = 0;
		
		public function rotateByAngleThumbImage(angle:int,image:Image):void
		{
			//trace('ANGLE :::: ' + angle);
			rotationAngle += angle;
			var offsetWidth:Number = image.width/2.0;
			var offsetHeight:Number =  image.height/2.0;
			var radians:Number = angle * (Math.PI / 180.0);
			var tempHeight:Number =  image.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(image.transform.matrix );
			image.transform.matrix = matrix;		
		}
		
		public function rotateByAngle(angle:int,image:Image):void
		{
			//rotationAngle += angle;
			var offsetWidth:Number = image.width/2.0;
			var offsetHeight:Number =  image.height/2.0;
			var radians:Number = angle * (Math.PI / 180.0);
			var tempHeight:Number =  image.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(matrixReal);
			matrixReal = matrix;
			image.transform.matrix = matrix;
		}
		
		public function rotateByAngleCompass(angle:int,image:Image):void
		{
			//rotationAngle += angle;
			var offsetWidth:Number = image.width/2.0;
			var offsetHeight:Number =  image.height/2.0;
			var radians:Number = angle * (Math.PI / 180.0);
			var tempHeight:Number =  image.height;
			var matrix:Matrix = new Matrix();
			matrix.translate(-offsetWidth, -offsetHeight);
			matrix.rotate(radians);
			matrix.translate(+offsetWidth, +offsetHeight);
			matrix.concat(image.transform.matrix );
			image.transform.matrix = matrix;			
		}
	}
}