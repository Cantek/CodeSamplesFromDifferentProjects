package imagecropper 
{	
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.Loader;
	import flash.display.Shape;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.MouseEvent;
	import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	import flash.net.URLRequest;
	
	import mx.core.UIComponent;
	import mx.managers.CursorManager;
	
	[Event(name="sourceImageLoading")]
	
	
	[Event(name="sourceImageLoadError")]
	
	
	[Event(name="sourceImageReady")]
	
	
	[Event(name="cropRectChanged")]
	
	
	[Event(name="cropConstraintChanged")]

	
	[Event(name="cropConstraintDisabled")]
	
	
	[Event(name="cropPositionChanged")]
	
	
	[Event(name="cropDimensionsChanged")]
		
	/** ImageCropper: This component is created by Cantek ÇETİN
	 *  ImageCropper for cropping desired area on image
	 */
 
	public class ImageCropper extends UIComponent {
		
		private var componentEnabled:Boolean = true;
		
		[Embed(source="hand.gif")]
		private var handCursor:Class;
		private var handCursorID:int = -1;
		
		private var imageSource:Object = null;
		
		private var componentWidth:Number;
		private var componentHeight:Number;
		
		private var componentBitmap:Bitmap;
		private var componentBitmapData:BitmapData;
		
		private var newImageLoaded:Boolean = false;
		private var currentSource:Loader;
		
		private var bkgndColor:uint = 0xFF000000;
		private var bkgndAlpha:uint = 0xFF000000;
		private var cropMaskColor:uint = 0x4CFF0000;
		private var cropMaskAlpha:uint = 0x4C000000;
		private var cropHandleColor:uint = 0xFFFF0000;
		private var cropHandleAlpha:Number = .5;
		private var cropSelectionOutlineColor:uint = 0xFFFFFFFF;
		private var cropSelectionOutlineAlpha:Number = 1.; 
		
		private var imageLocation:Point;
		private var imageScaleFactor:Number;
		private var imageScaledWidth:Number;
		private var imageScaledHeight:Number;
		private var imageBitmapData:BitmapData;
		private var scaledImageBitmapData:BitmapData;
		
		private var cropX:Number;
		private var cropY:Number;
		private var cropWidth:Number;
		private var cropHeight:Number;		
		private var newCroppingRect:Boolean = false;
		private var cropRatioActive:Boolean = false;
		private var cropRatio:Number = 0;
		private var cropRect:Rectangle;	
		private var croppingRectBitmapData:BitmapData;
		private var anchorX:Number;
		private var anchorY:Number;
		private var activeHandle:int;
		private var croppingRectIsImageScale:Boolean;
		private var cropMaskChanged:Boolean = false;
		
		private var cropHandleSize:Number = 10;
		private var cropRectMinimumSize:Number = 20;
		
		private var destroyed:Boolean = false;
		
		private var mouseListenersActive:Boolean = false;
		
		public const SOURCE_IMAGE_LOADING:String = "sourceImageLoading";
		
		
		public const SOURCE_IMAGE_LOAD_ERROR:String = "sourceImageLoadError";
		
		public const SOURCE_IMAGE_READY:String = "sourceImageReady";
		
		public const CROP_RECT_CHANGED:String = "cropRectChanged";
		
		
		public const CROP_CONSTRAINT_CHANGED:String = "cropConstraintChanged";
		
		
		public const CROP_CONSTRAINT_DISABLED:String = "cropConstraintDisabled";
		
		
		public const CROP_POSITION_CHANGED:String = "cropPositionChanged";
		
		public const CROP_DIMENSIONS_CHANGED:String = "cropDimensionsChanged";		
		
		
		public const VERSION:Number = 1.0;		
		
		
		public function ImageCropper() {
						
			super();
		}
		
		
		public function destroy():void {
			
			destroyed = true;
			
			if (componentBitmapData != null) {
				componentBitmapData.dispose();
				componentBitmapData = null;
				componentBitmap = null;
			}
			
			if (imageBitmapData != null) {
				imageBitmapData.dispose();
				imageBitmapData = null;
			}			
			
			if (scaledImageBitmapData != null) {
				scaledImageBitmapData.dispose();
				scaledImageBitmapData = null;
			}
			
			if (handCursorID >= 0) {
				CursorManager.removeCursor(handCursorID);
				handCursorID = -1;
			}
			
			activateListeners(false); 
		}
		
		
		public override function get enabled():Boolean {
			
			return componentEnabled;
		}	

		public override function set enabled(value:Boolean):void {
			
			if (componentEnabled != value) {
				
				super.enabled = value;
				
				componentEnabled = value;
				
				if (imageBitmapData != null) activateListeners(componentEnabled);
				
				if (handCursorID >= 0) {
					CursorManager.removeCursor(handCursorID);
					handCursorID = -1;
				}								
				
				invalidateDisplayList();
			}
		}
		
		
		public function get backgroundColor():uint {
			return (bkgndColor & 0x00FFFFFF);
		}	
		
		public function set backgroundColor(value:uint):void {
			
			
			if (componentEnabled) {
				
				bkgndColor = value;
								
				bkgndColor &= 0x00FFFFFF;
				bkgndColor |= bkgndAlpha;
				
				invalidateDisplayList();
			}
		}
		
		
		public function get backgroundAlpha():Number {
			if (bkgndAlpha > 0) return (bkgndAlpha >> 48) / 100;
			else return 0;
		}
		
		public function set backgroundAlpha(value:Number):void {
						
			if (componentEnabled) {
				
				if (value > 1) value = 1;
				else if (value < 0) value = 0;
				
				var a:uint = Math.round(value * 255);
				bkgndAlpha = (Math.round(value * 255)) << 24;
				
				bkgndColor &= 0x00FFFFFF;
				bkgndColor |= bkgndAlpha;
				
				invalidateDisplayList();
			}
		}
		
		
		public function get maskColor():uint {
			return (cropMaskColor & 0x00FFFFFF);
		}	
		
		public function set maskColor(value:uint):void {
			
			if (componentEnabled) {
				
				cropMaskColor = value;
				
				cropMaskColor &= 0x00FFFFFF;
				cropMaskColor |= cropMaskAlpha;
				
				cropMaskChanged = true;
				invalidateProperties();
			}
		}
		
		
		public function get maskAlpha():Number {
			if (cropMaskAlpha > 0) return (cropMaskAlpha >> 48) / 100;
			else return 0;
		}				
		
		
		public function set maskAlpha(value:Number):void {
			
			if (componentEnabled) {
				
				if (value > 1) value = 1;
				else if (value < 0) value = 0;
				
				var a:uint = Math.round(value * 255);
				cropMaskAlpha = (Math.round(value * 255)) << 24;
				
				cropMaskColor &= 0x00FFFFFF;
				cropMaskColor |= cropMaskAlpha;
				
				cropMaskChanged = true;
				invalidateProperties();
			}
		}		
		
		public function get handleColor():uint {
			return (cropHandleColor & 0x00FFFFFF);
		}	
		
		public function set handleColor(value:uint):void {
			
			
			if (componentEnabled) {
				
				cropHandleColor = value;
				
				cropHandleColor &= 0x00FFFFFF;
				cropHandleColor |= cropHandleAlpha;
				
				cropMaskChanged = true;
				invalidateProperties();
			}
		}		
		
		public function get handleAlpha():Number {
			return cropHandleAlpha;
		}
		
		public function set handleAlpha(value:Number):void {
			
			if (componentEnabled) {
				
				if (value > 1) value = 1;
				else if (value < 0) value = 0;
				
				cropHandleAlpha = value;
				
				cropMaskChanged = true;
				invalidateProperties();
			}
		}
		
		public function get outlineColor():uint {
			return cropSelectionOutlineColor;
		}
		
		
		public function set outlineColor(value:uint):void {
			
			if (componentEnabled) {
				
				cropSelectionOutlineColor = value;
				
				cropMaskChanged = true;
				invalidateProperties();
			}
		}		
		
		public function get outlineAlpha():Number {
			return cropSelectionOutlineAlpha;
		}
		
		public function set outlineAlpha(value:Number):void {
			
			if (componentEnabled) {
				
				if (value > 1) value = 1;
				else if (value < 0) value = 0;
				
				cropSelectionOutlineAlpha = value;
				
				cropMaskChanged = true;
				invalidateProperties();
			}
		}
		
		public function get handleSize():Number {
			return cropHandleSize;
		}
		public function set handleSize(value:Number):void {
			
			if (componentEnabled) {
				
				if (handCursorID >= 0) {
					CursorManager.removeCursor(handCursorID);
					handCursorID = -1;
				}
				
				cropHandleSize = value < 3 ? 3 : value;
				
				cropRectMinimumSize = value * 2;
				
				cropMaskChanged = true;
				invalidateProperties();
			}
		}
		
		
		public function get constrainToAspectRatio():Boolean {
			return cropRatioActive;
		}
		
		
		public function set constrainToAspectRatio(constrain:Boolean):void {
			
			if (componentEnabled) {
				
				if (constrain) {				
					if (cropRect != null) cropRatio = cropRect.width / cropRect.height;
					else cropRatio = 0;
				}
				else cropRatio = 0;
				
				cropRatioActive = constrain;
			}
		}		
		
		
		public function get sourceImage():Object {
			return imageSource;
		}
		
		
		public function set sourceImage(value:Object):void {
			
			if (componentEnabled) {
				
				if (value != null && (value is String || value is BitmapData)) {
					
					if (handCursorID >= 0) {
						CursorManager.removeCursor(handCursorID);
						handCursorID = -1;
					}
					if (imageBitmapData != null) {
						if (componentBitmap != null) componentBitmapData.fillRect(componentBitmapData.rect, bkgndColor);		
						imageBitmapData.dispose();
						imageBitmapData = null;
						imageSource = null;
						activateListeners(false);
					}
					if (value is String) {
						currentSource = new Loader();
						sourceLoadListeners(true);
						currentSource.load(new URLRequest(String(value)));
						dispatchEvent(new Event(SOURCE_IMAGE_LOADING));					
					}
					else {
						
						imageBitmapData = BitmapData(value).clone();
						
						newImageLoaded = true;			
						
						invalidateDisplayList();
						
						imageSource = imageBitmapData;
						
						activateListeners(true);
						
						dispatchEvent(new Event(SOURCE_IMAGE_READY));
					}
				}
			}
		}
		
		
		public function get croppedBitmapData():BitmapData {
			
			if (componentBitmapData == null) initializeDisplay(this.width, this.height);
			
			if (newImageLoaded) createScaledImage();		
			
			if (newCroppingRect) initializeCroppingRect();
			
			var croppedBitmap:BitmapData = null;
			var sourceImageRect:Rectangle = getCropRect();
			
			if (imageBitmapData != null && sourceImageRect != null && sourceImageRect.width > 0 && sourceImageRect.height > 0) {
				croppedBitmap = new BitmapData(sourceImageRect.width, sourceImageRect.height, false);
				croppedBitmap.copyPixels(imageBitmapData, sourceImageRect, new Point(0, 0));
			}
			
			return croppedBitmap;
		}
				
		public function setCropRect(width:Number = 0, height:Number = 0, x:Number = -1, y:Number = -1, componentRelative:Boolean = false):void {
			
			if (componentEnabled) 
			{
				
				if (handCursorID >= 0) {
					CursorManager.removeCursor(handCursorID);
					handCursorID = -1;
				}		
				if (width < 0) width = 0;
				if (height < 0) height = 0;
				if (x < -1) x = -1;
				if (y < -1) y = -1;
				cropX = x;
				cropY = y;
				cropWidth = width;
				cropHeight = height;
				if (cropWidth == 0 || cropHeight == 0) {
					
					cropWidth = 0;
					cropHeight = 0;
					
					if (cropRatioActive) {
						
						if (scaledImageBitmapData != null) {
							cropWidth = scaledImageBitmapData.width - 1;
							cropHeight = scaledImageBitmapData.height - 1;
						}
						else {
							dispatchEvent(new Event(CROP_CONSTRAINT_DISABLED));
							cropRatioActive = false;
						}
					}
				}
				
				croppingRectIsImageScale = !componentRelative;
				
				if (cropRatioActive) cropRatio = cropWidth / cropHeight;
				else cropRatio = 0;
				
				newCroppingRect = true;
				
				invalidateDisplayList();
			}		
		}	
		
		public function getCropRect(componentRelative:Boolean = false, roundValues:Boolean = false):Rectangle {
			
			if (cropRect != null) {
				
				var requestedRect:Rectangle;
				if (componentRelative) requestedRect = cropRect.clone();
					
				else requestedRect = new Rectangle(cropRect.x / imageScaleFactor, cropRect.y / imageScaleFactor, cropRect.width / imageScaleFactor, cropRect.height / imageScaleFactor);
				
				if (roundValues) {
					requestedRect.x = Math.round(requestedRect.x);
					requestedRect.y = Math.round(requestedRect.y);
					requestedRect.width = Math.round(requestedRect.width);
					requestedRect.height = Math.round(requestedRect.height);
				}
				return requestedRect; 
			}
				
			else return null;
		}								
		
		
		private function sourceLoadSuccess(event:Event):void 
		{
			try
			{
				imageBitmapData = Bitmap(currentSource.content).bitmapData.clone();
				sourceLoadListeners(false);
				imageSource = imageBitmapData;				
				newImageLoaded = true;
				invalidateDisplayList();				
				activateListeners(true);
				dispatchEvent(new Event(SOURCE_IMAGE_READY));
			}
			catch(error:Error)
			{}
		}		
		
		private function sourceLoadFailure(event:IOErrorEvent):void {
			
			sourceLoadListeners(false);
			dispatchEvent(new Event(SOURCE_IMAGE_LOAD_ERROR));								
		}		
		
		private function sourceLoadListeners(addListeners:Boolean):void {
			
			if (addListeners) {
				
				currentSource.contentLoaderInfo.addEventListener(Event.INIT, sourceLoadSuccess);
				
				currentSource.contentLoaderInfo.addEventListener(IOErrorEvent.IO_ERROR, sourceLoadFailure);
			}
				
			else {
				
				currentSource.contentLoaderInfo.removeEventListener(Event.INIT, sourceLoadSuccess);
				currentSource.contentLoaderInfo.removeEventListener(IOErrorEvent.IO_ERROR, sourceLoadFailure);
				
				try {
					currentSource.close();
				}
				catch(e:Error) {};
				
				currentSource.unload();
				currentSource = null;				
			}	
		}		
		
		
		private function activateListeners(addListeners:Boolean):void {
			
			if (addListeners && !mouseListenersActive) {
				this.addEventListener(MouseEvent.MOUSE_DOWN, doMouseDown);
				this.addEventListener(MouseEvent.MOUSE_MOVE, doMouseMove);
				mouseListenersActive = true;
			}
				
			else if (!addListeners && mouseListenersActive) {
				this.removeEventListener(MouseEvent.MOUSE_MOVE, doMouseMove);
				this.removeEventListener(MouseEvent.MOUSE_DOWN, doMouseDown);
				this.removeEventListener(MouseEvent.MOUSE_UP, doMouseUp);
				systemManager.stage.removeEventListener(Event.MOUSE_LEAVE, doMouseUp);
				systemManager.stage.removeEventListener(MouseEvent.MOUSE_UP, doMouseUp);
				mouseListenersActive = false;
				if (currentSource != null) sourceLoadListeners(false);
			}		
		}
		
		private function mouseLocation(event:MouseEvent):int {
			
			var mouseXLoc:Number = event.localX - imageLocation.x;
			var mouseYLoc:Number = event.localY - imageLocation.y;
			if (cropRect != null && cropRect.contains(mouseXLoc, mouseYLoc)) {
				var mouseDeltaX:Number = mouseXLoc - cropRect.x;
				var mouseDeltaY:Number = mouseYLoc - cropRect.y;
				
				if (mouseDeltaX <= cropHandleSize && mouseDeltaY <= cropHandleSize) return 1;
					
				else if (mouseDeltaX >= cropRect.width - cropHandleSize && mouseDeltaY <= cropRect.width && mouseDeltaY <= cropHandleSize) return 2;
				else if (mouseDeltaX <= cropHandleSize && mouseDeltaY >= cropRect.height - cropHandleSize && mouseDeltaY <= cropRect.height) return 3;
					
				else if (mouseDeltaX >= cropRect.width - cropHandleSize && mouseDeltaY <= cropRect.height && mouseDeltaY >= cropRect.height - cropHandleSize && mouseDeltaY <= cropRect.height) return 4;
					
				else return 0;
			}
			else return -1;
		}								
		
		private function doMouseDown(event:MouseEvent):void {
			var mouseLoc:int = mouseLocation(event);			
			
			if (mouseLoc >= 0) {
				
				var mouseDeltaX:Number = event.localX - imageLocation.x - cropRect.x;
				var mouseDeltaY:Number = event.localY - imageLocation.y - cropRect.y;
				
				if (mouseLoc == 1) {
					anchorX = mouseDeltaX;
					anchorY = mouseDeltaY;					
					activeHandle = 1;
				}					
					
				else if (mouseLoc == 2) {
					anchorX = mouseDeltaX - cropRect.width;
					anchorY = mouseDeltaY;	
					activeHandle = 2;
				}					
					
				else if (mouseLoc == 3) {
					anchorX = mouseDeltaX;
					anchorY = mouseDeltaY - cropRect.height;	
					activeHandle = 3;
				}
				else if (mouseLoc == 4) {
					anchorX = mouseDeltaX - cropRect.width;
					anchorY = mouseDeltaY - cropRect.height;	
					activeHandle = 4;
				}
				else {
					anchorX = mouseDeltaX;
					anchorY = mouseDeltaY;
					activeHandle = 0;
				}
				
				this.addEventListener(MouseEvent.MOUSE_UP, doMouseUp);
				systemManager.stage.addEventListener(Event.MOUSE_LEAVE, doMouseExit);
				systemManager.stage.addEventListener(MouseEvent.MOUSE_UP, doMouseUp);
			}
		}
		
		private function doMouseExit(event:Event):void {
			doMouseUp(null);
		}
		
		private function doMouseUp(event:MouseEvent):void {
			
			activeHandle = -1;
			
			this.removeEventListener(MouseEvent.MOUSE_UP, doMouseUp);
			systemManager.stage.removeEventListener(Event.MOUSE_LEAVE, doMouseUp);
			systemManager.stage.removeEventListener(MouseEvent.MOUSE_UP, doMouseUp);
			
			dispatchEvent(new Event(CROP_RECT_CHANGED));
		}		
		
		
		private function doMouseMove(event:MouseEvent):void {
			
			if (croppingRectBitmapData != null && event.buttonDown) {
				
				var topX:Number;
				var topY:Number;
				var btmX:Number;
				var btmY:Number;
				
				var scaledW:Number;
				var scaledH:Number; 
				
				var mouseX:Number = event.localX - imageLocation.x;
				var mouseY:Number = event.localY - imageLocation.y;
				
				if (activeHandle == 0) {
					
					cropRect.x = mouseX - anchorX;
					cropRect.y = mouseY - anchorY;
					
					if (cropRect.x < 0) cropRect.x = 0;
					else {
						var maxX:Number = Math.floor(imageScaledWidth - cropRect.width - 1);
						if (cropRect.x > maxX) cropRect.x = maxX;
					}
					
					if (cropRect.y < 0) cropRect.y = 0;
					else {
						var maxY:Number = Math.floor(imageScaledHeight - cropRect.height - 1);
						if (cropRect.y > maxY) cropRect.y = maxY;
					}
				}
					
				else if (activeHandle == 1) {
					
					// Get the new position of the top-left corner of the cropping rectangle
					
					topX = mouseX - anchorX;
					topY = mouseY - anchorY;
					
					// If the new position of the top-left corner of the cropping rectangle is outside the top or left margin, then set the corner to the top or left margin
					
					if (topX < 0) topX = 0;
					if (topY < 0) topY = 0;
					
					// Get the current position of the bottom-right corner of the cropping rectangle
					
					btmX = cropRect.x + cropRect.width;
					btmY = cropRect.y + cropRect.height;
					
					// Calculate the new width of the cropping rectangle
					
					cropRect.width = btmX - topX;
					
					// If the new width is less than the minimum allowed size then set the new width to the minimum
					
					if (cropRect.width < cropRectMinimumSize) {
						cropRect.width = cropRectMinimumSize;
						topX = btmX - cropRectMinimumSize;
					}
					
					// Set the new x position for the top-left corner of the cropping rectangle
					
					cropRect.x = topX;
					
					// Calculate the new height of the cropping rectangle
					
					cropRect.height = btmY - topY;
					
					// If the new height is less than the minimum allowed size then set the new height to the minimum
					
					if (cropRect.height < cropRectMinimumSize) {
						cropRect.height = cropRectMinimumSize;
						cropRect.y = btmY - cropRectMinimumSize;
					}
					else cropRect.y = topY;
					
					// If a cropping ratio is defined
					
					if (cropRatioActive) {
						
						// If the cropping rectangle is not at the correct ratio
						
						if (cropRect.width / cropRect.height != cropRatio) {
							
							// Get a new scaled width for the cropping rectangle
							
							scaledW = cropRect.height * cropRatio;
							
							// If the new width is less then the minimum allowed width
							
							if (scaledW < cropRectMinimumSize) {
								
								// Set the width to the minimum
								
								scaledW = cropRectMinimumSize;
								
								// Scale the height for the new width
								
								scaledH = scaledW / cropRatio;
								
								// Adjust the y coordinate of the cropping rectangle for the new scaled height
								
								cropRect.y = cropRect.y + (cropRect.height - scaledH);
								
								// Set the new height of the cropping rectangle
								
								cropRect.height = scaledH;
							}
							
							// Move the horizontal position to compensate for the scaled width
							
							cropRect.x += cropRect.width - scaledW;
							
							// If the new width is to the left of the left margin
							
							if (cropRect.x < 0) {
								
								// Reduce the width of the cropping rectangle by the amount that the cropping rectangle extends past the left margin
								
								scaledW += cropRect.x;
								
								// Set the left edge of the cropping rectangle to the left margin
								
								cropRect.x = 0;
								
								// Scale the height for the new width
								
								scaledH = scaledW / cropRatio;
								
								// Adjust the y coordinate of the cropping rectangle for the new scaled height
								
								cropRect.y = cropRect.y + (cropRect.height - scaledH);
								
								// Set the new height of the cropping rectangle
								
								cropRect.height = scaledH;
							}
							
							// Set the scaled width for the cropping rectangle
							
							cropRect.width = scaledW;														
						}
					}					
				}
					
					// Top-right Handle
					
				else if (activeHandle == 2) {
					
					// Get the new position of the top-right corner of the cropping rectangle
					
					topX = mouseX - anchorX;
					topY = mouseY - anchorY;
					
					// Get the current position of the bottom-left corner of the cropping rectangle
					
					btmX = cropRect.x;
					btmY = cropRect.y + cropRect.height;
					
					// If the new position of the top-right corner of the cropping rectangle is outside the top or right margin, then set the corner to the top or right margin
					
					if (topX > imageScaledWidth - 1) topX = imageScaledWidth - 1;
					if (topY < 0) topY = 0;
					
					// Calculate the new width of the cropping rectangle
					
					cropRect.width = topX - btmX;
					
					// If the new width is less than the minimum allowed size then set the new width to the minimum
					
					if (cropRect.width < cropRectMinimumSize) {
						cropRect.width = cropRectMinimumSize;
						topX = btmX + cropRectMinimumSize;
					}
					
					// Set the new x position for the top-left corner of the cropping rectangle
					
					cropRect.x = btmX;
					
					// Calculate the new height of the cropping rectangle
					
					cropRect.height = btmY - topY;
					
					// If the new height is less than the minimum allowed size then set the new height to the minimum
					
					if (cropRect.height < cropRectMinimumSize) {
						cropRect.height = cropRectMinimumSize;
						cropRect.y = btmY - cropRectMinimumSize;
					}
					else cropRect.y = topY;
					
					// If a cropping ratio is defined
					
					if (cropRatioActive) {
						
						// If the cropping rectangle is not at the correct ratio
						
						if (cropRect.width / cropRect.height != cropRatio) {
							
							// Get a new scaled width for the cropping rectangle
							
							scaledW = cropRect.height * cropRatio;
							
							// If the new width is less then the minimum allowed width
							
							if (scaledW < cropRectMinimumSize) {
								
								// Set the width to the minimum
								
								scaledW = cropRectMinimumSize;
								
								// Scale the height for the new width
								
								scaledH = scaledW / cropRatio;
								
								// Adjust the y coordinate of the cropping rectangle for the new scaled height
								
								cropRect.y = cropRect.y + (cropRect.height - scaledH);
								
								// Set the new height of the cropping rectangle
								
								cropRect.height = scaledH;
							}
							
							// If the new width exceeds the maximum allowed width
							
							if (cropRect.x + scaledW > imageScaledWidth - 1) {
								
								// Set the width to the maximum allowed
								
								scaledW = imageScaledWidth - 1 - cropRect.x;
								
								// Scale the height for the new width
								
								scaledH = scaledW / cropRatio;
								
								// Adjust the y coordinate of the cropping rectangle for the new scaled height
								
								cropRect.y = cropRect.y + (cropRect.height - scaledH);
								
								// Set the new height of the cropping rectangle
								
								cropRect.height = scaledH;
							}
							
							// Set the scaled width for the cropping rectangle
							
							cropRect.width = scaledW;							
						}
					}
				}
					
					// Bottom-left Handle
					
				else if (activeHandle == 3) {
					
					// Get the new position of the bottom-left corner of the cropping rectangle
					
					btmX = mouseX - anchorX;
					btmY = mouseY - anchorY;
					
					// If the new position of the bottom-left corner of the cropping rectangle is outside the bottom or left margin, then set the corner to the bottom or left margin
					
					if (btmX < 0) btmX = 0;
					if (btmY > imageScaledHeight - 1) btmY = imageScaledHeight - 1;
					
					// Get the current position of the top-right corner of the cropping rectangle
					
					topX = cropRect.x + cropRect.width;
					topY = cropRect.y;
					
					// Calculate the new width of the cropping rectangle
					
					cropRect.width = topX - btmX;
					
					// If the new width is less than the minimum allowed size then set the new width to the minimum
					
					if (cropRect.width < cropRectMinimumSize) {
						cropRect.width = cropRectMinimumSize;
						btmX = topX - cropRectMinimumSize;
					}
					
					// Set the new x position for the top-left corner of the cropping rectangle
					
					cropRect.x = btmX;
					
					// Calculate the new height of the cropping rectangle
					
					cropRect.height = btmY - topY;
					
					// If the new height is less than the minimum allowed size then set the new height to the minimum
					
					if (cropRect.height < cropRectMinimumSize) cropRect.height = cropRectMinimumSize;
					
					// Set the new y position for the top-left corner of the cropping rectangle
					
					cropRect.y = topY;
					
					// If a cropping ratio is defined
					
					if (cropRatioActive) {
						
						// If the cropping rectangle is not at the correct ratio
						
						if (cropRect.width / cropRect.height != cropRatio) {
							
							// Get a new scaled width for the cropping rectangle
							
							scaledW = cropRect.height * cropRatio;
							
							// If the new width is less then the minimum allowed width
							
							if (scaledW < cropRectMinimumSize) {
								
								// Set the width to the minimum
								
								scaledW = cropRectMinimumSize;
								
								// Scale the height for the new width
								
								scaledH = scaledW / cropRatio;
								
								// Set the new height of the cropping rectangle
								
								cropRect.height = scaledH;
							}
							
							// Move the horizontal position to compensate for the scaled width
							
							cropRect.x += cropRect.width - scaledW;
							
							// If the new width is to the left of the left margin
							
							if (cropRect.x < 0) {
								
								// Reduce the width of the cropping rectangle by the amount that the cropping rectangle extends past the left margin
								
								scaledW += cropRect.x;
								
								// Set the left edge of the cropping rectangle to the left margin
								
								cropRect.x = 0;
								
								// Scale the height for the new width
								
								scaledH = scaledW / cropRatio;
								
								// Set the new height of the cropping rectangle
								
								cropRect.height = scaledH;
							}
							
							// Set the scaled width for the cropping rectangle
							
							cropRect.width = scaledW;														
						}
					}					
				}
					
					// Bottom-right Handle				
					
				else if (activeHandle == 4) {
					
					// Get the new position of the bottom-left corner of the cropping rectangle
					
					btmX = mouseX - anchorX;
					btmY = mouseY - anchorY;
					
					// If the new position of the bottom-right corner of the cropping rectangle is outside the bottom or right margin, then set the corner to the bottom or right margin
					
					if (btmX > imageScaledWidth - 1) btmX = imageScaledWidth - 1;
					if (btmY > imageScaledHeight - 1) btmY = imageScaledHeight - 1;
					
					// Get the current position of the top-left corner of the cropping rectangle
					
					topX = cropRect.x;
					topY = cropRect.y;
					
					// Calculate the new width of the cropping rectangle
					
					cropRect.width = btmX - topX;
					
					// If the new width is less than the minimum allowed size then set the new width to the minimum
					
					if (cropRect.width < cropRectMinimumSize) {
						cropRect.width = cropRectMinimumSize;
						btmX = topX + cropRectMinimumSize;
					}
					
					// Calculate the new height of the cropping rectangle
					
					cropRect.height = btmY - topY;
					
					// If the new height is less than the minimum allowed size then set the new height to the minimum
					
					if (cropRect.height < cropRectMinimumSize) cropRect.height = cropRectMinimumSize;
					
					// If a cropping ratio is defined
					
					if (cropRatioActive) {
						
						// If the cropping rectangle is not at the correct ratio
						
						if (cropRect.width / cropRect.height != cropRatio) {
							
							// Get a new scaled width for the cropping rectangle
							
							scaledW = cropRect.height * cropRatio;
							
							// If the new width is less then the minimum allowed width
							
							if (scaledW < cropRectMinimumSize) {
								
								// Set the width to the minimum
								
								scaledW = cropRectMinimumSize;
								
								// Scale the height for the new width
								
								scaledH = scaledW / cropRatio;
								
								// Set the new height of the cropping rectangle
								
								cropRect.height = scaledH;
							}
							
							// If the new width exceeds the maximum allowed width
							
							if (cropRect.x + scaledW > imageScaledWidth - 1) {
								
								// Set the width to the maximum allowed
								
								scaledW = imageScaledWidth - 1 - cropRect.x;
								
								// Scale the height for the new width
								
								scaledH = scaledW / cropRatio;
								
								// Set the new height of the cropping rectangle
								
								cropRect.height = scaledH;
							}
							
							// Set the scaled width for the cropping rectangle
							
							cropRect.width = scaledW;							
						}
					}					
				}		
				
				// Render the cropping rectangle on top of the image
				
				drawCroppingRect();
				
				// Invalidate the display list to redraw the component
				
				invalidateDisplayList();				
			}
				
				// Else a handle is not active
				
			else {
				
				// If the cropping rectangle is smaller than the image dimensions and if the mouse is within the cropping rectangle but not in a handle then show the hand cursor
				
				if ((cropRect.width + 1 < Math.floor(imageScaledWidth) || cropRect.height + 1 < Math.floor(imageScaledHeight)) && mouseLocation(event) == 0) {
					if (handCursorID < 0) handCursorID = CursorManager.setCursor(handCursor, 2, -6, -3);
				}
					
					// Else the mouse is in a handle or outside of the cropping rectangle
					
				else {
					
					// If the hand cursor is active, remove it
					
					if (handCursorID >= 0) {
						CursorManager.removeCursor(handCursorID);
						handCursorID = -1;
					}										
				}
			}
		}
		
		// --------------------------------------------------------------------------------------------------
		// drawCroppingRect
		// --------------------------------------------------------------------------------------------------
		
		private function drawCroppingRect():void {
			
			// If the BitmapData object has been constructed, proceed to draw the cropping rectangle
			
			if (croppingRectBitmapData && componentEnabled) {
				
				// Clear the previous cropping rectangle by filling the bitmap with the mask color
				
				croppingRectBitmapData.fillRect(croppingRectBitmapData.rect, cropMaskColor);
				
				// Draw the transparent cropping rectangle area
				
				croppingRectBitmapData.fillRect(cropRect, 0x00FFFFFF);
				
				// Draw boarder around the cropping rectangle
				
				var border:Shape = new Shape();
				border.graphics.lineStyle(1, cropSelectionOutlineColor, cropSelectionOutlineAlpha);
				border.graphics.drawRect(cropRect.x, cropRect.y, cropRect.width, cropRect.height);
				croppingRectBitmapData.draw(border);	
				
				// Draw corner handles
				
				var handles:Shape = new Shape();
				handles.graphics.lineStyle(1, cropSelectionOutlineColor, cropSelectionOutlineAlpha);
				
				handles.graphics.beginFill(cropHandleColor, cropHandleAlpha);
				handles.graphics.drawRect(cropRect.x, cropRect.y, cropHandleSize, cropHandleSize);
				handles.graphics.endFill();
				
				handles.graphics.beginFill(cropHandleColor, cropHandleAlpha);
				handles.graphics.drawRect(cropRect.x + cropRect.width - cropHandleSize, cropRect.y, cropHandleSize, cropHandleSize);
				handles.graphics.endFill();
				
				handles.graphics.beginFill(cropHandleColor, cropHandleAlpha);
				handles.graphics.drawRect(cropRect.x, cropRect.y + cropRect.height - cropHandleSize, cropHandleSize, cropHandleSize);
				handles.graphics.endFill();
				
				handles.graphics.beginFill(cropHandleColor, cropHandleAlpha);
				handles.graphics.drawRect(cropRect.x + cropRect.width - cropHandleSize, cropRect.y + cropRect.height - cropHandleSize, cropHandleSize, cropHandleSize);
				handles.graphics.endFill();
				
				croppingRectBitmapData.draw(handles);
			}
		}		
		
		// --------------------------------------------------------------------------------------------------
		// commitProperties - Handle cropping rectangle property change (called by Flex when invalidateProperties is called after a change is made to a cropping rectangle property)
		// --------------------------------------------------------------------------------------------------
		
		/**
		 * @private
		 */
		
		override protected function commitProperties():void {
			
			super.commitProperties();
			
			// If a property for the cropping rectangle has been changed
			
			if (cropMaskChanged) {
				
				// Reset property changed flag
				
				cropMaskChanged = false;
				
				// Check the cropping rectangle size to make sure that there is enough space to display the handles with no overlap; if there isn't, then increase the size of the cropping rectangle.
				
				if (cropRect != null && (cropRectMinimumSize > cropRect.width || cropRectMinimumSize > cropRect.height)) {
					
					var origRect:Rectangle = cropRect.clone();
					
					if (cropRect.y > imageScaledHeight - 1 - cropRectMinimumSize) cropRect.y = imageScaledHeight - 1 - cropRectMinimumSize;
					if (cropRect.x > imageScaledWidth - 1 - cropRectMinimumSize) cropRect.x = imageScaledWidth - 1 - cropRectMinimumSize;
					if (cropRect.height < cropRectMinimumSize) cropRect.height = cropRectMinimumSize;
					if (cropRect.width < cropRectMinimumSize) cropRect.width = cropRectMinimumSize;
					
					// If the aspect ratio is constrained, make sure that it doesn't change unless it's not possible to maintain the aspect ratio in the available space (given the current crop handle size)
					
					if (constrainToAspectRatio && cropRect.width != cropRect.height * cropRatio) {
						
						// Calculate what the new cropping rectangle width or the new cropping rectangle height will have to be in order to maintain the correct aspect ratio
						// (the dimension that is not less than the cropping rectangle cropRectMinimumSize will be used)
						
						var newWidth:Number = cropRect.height * cropRatio;
						var newHeight:Number = cropRect.width / cropRatio;
						
						// Increase width
						
						if (newWidth > cropRectMinimumSize) {
							
							// Set the cropping rectangle to the width that will maintain the aspect ratio
							
							cropRect.width = newWidth;
							
							// If the new width causes the crop box to extend beyond the right edge ...
							
							if (cropRect.x + cropRect.width > imageScaledWidth - 1) {
								
								// Find out how far off the edge the cropping rectangle extends
								
								var wDelta:Number = (cropRect.x + cropRect.width) - (imageScaledWidth - 1);
								
								// If there is room to the left of the cropping rectangle, adjust the left edge of the cropping rectangle so it fits in the available space
								
								if (cropRect.x - wDelta >= 0) cropRect.x = cropRect.x - wDelta;
									
									// Else there is no way to maintain the aspect ratio so extend the cropping rectangle from the left edge to the right edge, calculate the new aspect ratio, and dispatch an event indicating that the ratio has been altered
									
								else {
									cropRect.x = 0;
									cropRect.width = imageScaledWidth - 1;
									cropRatio = cropRect.width / cropRect.height;
									dispatchEvent(new Event(CROP_CONSTRAINT_CHANGED));
								}
							}
						}
							
							// Increase height
							
						else {
							
							// Set the cropping rectangle to the height that will maintain the aspect ratio
							
							cropRect.height = newHeight;
							
							// If the new height causes the crop box to extend beyond the bottom edge ...
							
							if (cropRect.y + cropRect.height > imageScaledHeight - 1) {
								
								// Find out how far off the edge the cropping rectangle extends
								
								var hDelta:Number = (cropRect.y + cropRect.height) - (imageScaledHeight - 1);
								
								// If there is room above the cropping rectangle, adjust the top edge of the cropping rectangle so it fits in the available space
								
								if (cropRect.y - hDelta >= 0) cropRect.y = cropRect.y - hDelta;
									
									// Else there is no way to maintain the aspect ratio so extend the cropping rectangle from the top to the bottom, calculate the new aspect ratio, and dispatch an event indicating that the ratio has been altered
									
								else {
									cropRect.y = 0;
									cropRect.height = imageScaledHeight - 1;
									cropRatio = cropRect.width / cropRect.height;
									dispatchEvent(new Event(CROP_CONSTRAINT_CHANGED));
									
								}
							}							
						}
					}
					
					// Dispatch events if the size of the handles cause the position or dimensions of the cropping rectangle to change
					
					if (cropRect.x != origRect.x || cropRect.y != origRect.y) dispatchEvent(new Event(CROP_POSITION_CHANGED));
					if (cropRect.width != origRect.width || cropRect.height != origRect.height) dispatchEvent(new Event(CROP_DIMENSIONS_CHANGED));
				}
				
				// Redraw the cropping rectangle area
				
				drawCroppingRect();
				
				// Schedule a display list update
				
				invalidateDisplayList();
			}
		}
		
		// --------------------------------------------------------------------------------------------------
		// measure - Sets the default component size and the component's minimum size in pixels 
		// --------------------------------------------------------------------------------------------------
		
		/**
		 * @private
		 */
		
		override protected function measure():void {
			
			super.measure();
			
			if (!isNaN(componentWidth) && !isNaN(componentHeight)) {
				
				// Set default measurements
				
				measuredWidth = componentWidth;
				measuredHeight = componentHeight;
				
				// Set optional minimum size measurements
				
				measuredMinWidth = componentWidth;
				measuredMinHeight = componentHeight;
			}			
		}	
		
		// --------------------------------------------------------------------------------------------------
		// updateDisplayList - This method is called to size and position the children of the component based on all previous property and style settings.
		//                     It also draws any skins or graphic elements that the component uses. Note that the parent container determines the size of the component itself.
		// --------------------------------------------------------------------------------------------------
		
		/**
		 * @private
		 */
		
		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void {
			
			// The Flex callLaterDispatcher() can call updateDisplayList() after this component's destroy() method has been called but before it is garbage collected.
			// When destroy() is called the "destroyed" flag is set to prevent a null object reference error if updateDisplayList() is subsequently called.
			
			if (!destroyed) {
				
				super.updateDisplayList(unscaledWidth, unscaledHeight);
				
				// If the dimensions of the component have changed, create a new bitmap to match the components new size
				
				if (unscaledWidth != componentWidth || unscaledHeight != componentHeight) initializeDisplay(unscaledWidth, unscaledHeight);
				
				// If an image is loaded
				
				if (imageBitmapData != null) {	
					
					// If the image has not been rendered yet, create a scaled version of the image that will fit within the component
					
					if (newImageLoaded) createScaledImage();
					
					if (newCroppingRect) initializeCroppingRect();
					
					// Clear the current content
					
					componentBitmapData.fillRect(componentBitmapData.rect, bkgndColor);
					
					// Draw the scaled image in the component's display
					
					componentBitmapData.copyPixels(scaledImageBitmapData, scaledImageBitmapData.rect, imageLocation, null, null, true);
					
					// Draw the cropping overlay if the component is enabled
					
					if (componentEnabled && croppingRectBitmapData != null) componentBitmapData.copyPixels(croppingRectBitmapData, croppingRectBitmapData.rect, imageLocation, null, null, true);
				}
			}			
		}
		
		// --------------------------------------------------------------------------------------------------
		// initializeDisplay - Create the bitmap that represents the component's display
		// --------------------------------------------------------------------------------------------------
		
		private function initializeDisplay(newWidth:int, newHeight:int):void {
			
			if (newWidth > 0 && newHeight > 0) {
				
				// If a bitmap already exists (i.e., if the display area has been resized), remove the previous bitmap
				
				if (componentBitmap != null) {
					removeChild(componentBitmap);
					componentBitmapData.dispose();
				}
				
				// Create a Bitmap with smoothing enabled that matches the component's size and add it to the display list
				
				componentBitmapData = new BitmapData(newWidth, newHeight, true, bkgndColor);
				componentBitmap = new Bitmap(componentBitmapData);
				addChild(componentBitmap);
				
				// Save the current size of the component
				
				componentWidth = newWidth;
				componentHeight = newHeight;	
			}
		}		
		
		// --------------------------------------------------------------------------------------------------
		// initializeCroppingRect
		// --------------------------------------------------------------------------------------------------
		
		private function initializeCroppingRect():void {
			
			// The cropping rectangle cannot be initialized unless the imageCropper component is enabled and createScaledImage() has been called
			
			if (componentEnabled && scaledImageBitmapData != null) {
				
				// Determine if cropping rectangle is to be centered
				
				var centerCropRect:Boolean = (cropX == -1) || (cropY == -1);
				
				// Clear the initialization flag that was set when newCroppingRect() was called
				
				newCroppingRect = false;
				
				// If a cropping rectangle was previously defined, dispose of the BitmapData
				
				if (croppingRectBitmapData != null) croppingRectBitmapData.dispose();
				
				// Create the BitmapData for the cropping rectangle (the display area should match the image's display area)
				
				croppingRectBitmapData = new BitmapData(scaledImageBitmapData.width, scaledImageBitmapData.height, true, 0xAA000000);
				
				// If the width or height is zero, then the cropping rectangle will be set to the size of the image
				
				if (cropWidth == 0 || cropHeight == 0) {
					
					// Set width and height to the size of the image
					
					cropWidth = scaledImageBitmapData.width - 1;
					cropHeight = scaledImageBitmapData.height - 1;					
					
					// Save original cropping rectangle to determine if it had to be repositioned or resized in order to be valid
					
					var origRect:Rectangle = new Rectangle(cropX, cropY, cropWidth, cropHeight);
					
					// If the crop ratio is active, calculate the ratio and save it as the original ratio (in case it has to be changed later in this function)
					
					if (cropRatioActive) {
						cropRatio = cropWidth / cropHeight;
						var origCropRatio:Number = cropRatio;
					}
				}
					
					// If croppingRectIsImageScale is true, then scale the cropping rectangle dimensions so that it is drawn at the same scale as the selected image
					
				else if (croppingRectIsImageScale) {
					
					// If the crop ratio is active, save it as the original ratio (in case it has to be changed later in this function)
					
					if (cropRatioActive) origCropRatio = cropRatio;
					
					// If a specific location is defined for the cropping rectangle, convert the X and Y coordinates
					
					if (cropX >= 0 && cropY >= 0) {
						cropX *= imageScaleFactor;
						cropY *= imageScaleFactor;
					}
					
					// Convert width and height of the cropping rectangle
					
					cropWidth *= imageScaleFactor;
					cropHeight *= imageScaleFactor;
					
					// Save original cropping rectangle to determine if it had to be repositioned or resized in order to be valid
					
					origRect = new Rectangle(cropX, cropY, cropWidth, cropHeight);					
					
					// Make sure that the dimensions are not smaller than the size of the cropping rectangle handles
					
					if (cropWidth < cropRectMinimumSize) {
						cropHeight = cropHeight * (cropRectMinimumSize / cropWidth)
						cropWidth = cropRectMinimumSize;
					}
					
					if (cropHeight < cropRectMinimumSize) {
						cropWidth = cropWidth * (cropRectMinimumSize / cropHeight)
						cropHeight = cropRectMinimumSize;
					}
				}
					
					// Esle save original cropping rectangle to determine if it had to be repositioned or resized in order to be valid
					
				else origRect = new Rectangle(cropX, cropY, cropWidth, cropHeight);					
				
				// If the cropping rectangle is positioned absolutely (i.e., cropX and cropY > -1)
				
				if (!centerCropRect) {
					
					// If the absolute positioning causes the cropping rectangle to extend beyond the image area, try to reposition the cropping rectangle so that it touches rather than exceeds the image boundry
					
					if (cropX + cropWidth + 1 > scaledImageBitmapData.width) cropX = scaledImageBitmapData.width - cropWidth - 1;
					if (cropY + cropHeight + 1 > scaledImageBitmapData.height) cropY = scaledImageBitmapData.height - cropHeight - 1;
					
					// If the top or left-edge of the cropping rectangle extends beyond the top or left edge of the image area then decrease the height or width so that it fits within the image area
					// If a dimension of the cropping rectangle cannot fit, then set that dimension equal to the width or height of the image.
					
					if (cropX < 0) {
						cropWidth += cropX;
						if (cropWidth <= 0) cropWidth = scaledImageBitmapData.width - 1;
						cropX = 0;
					}
					
					if (cropY < 0) {
						cropHeight += cropY;
						if (cropHeight <= 0) cropHeight = scaledImageBitmapData.height - 1;
						cropY = 0;
					}
				}
				
				// Make sure that neither dimension exceeds the image dimensions
				
				if (cropWidth + 1 > scaledImageBitmapData.width) {
					cropWidth = scaledImageBitmapData.width - 1;
					if (cropRatioActive) cropHeight = cropWidth / cropRatio;
				}
				
				if (cropHeight + 1 > scaledImageBitmapData.height) {
					cropHeight = scaledImageBitmapData.height - 1;
					if (cropRatioActive) cropWidth = cropHeight * cropRatio;
				}
				
				// If the cropping rectangle is not absolutely positioned, then center the cropping rectangle in the image area
				
				if (centerCropRect) {
					cropX = (scaledImageBitmapData.width - cropWidth) / 2;
					cropY = (scaledImageBitmapData.height - cropHeight) / 2;
				}
				
				// If the absolute positioning causes the cropping rectangle to extend beyond the image area, try to reposition the cropping rectangle so that it touches rather than exceeds the image boundry
				
				if (cropX + cropWidth + 1 > scaledImageBitmapData.width) cropX = scaledImageBitmapData.width - cropWidth - 1;
				if (cropY + cropHeight + 1 > scaledImageBitmapData.height) cropY = scaledImageBitmapData.height - cropHeight - 1;
				
				// If adjusting cropX and cropY results in zero or negative space, reset the cropping rectangle to the image area
				
				if (scaledImageBitmapData.width - 1 <= cropX || scaledImageBitmapData.height - 1 <= cropY) {
					cropX = 0;
					cropY = 0;
					cropWidth = scaledImageBitmapData.width - 1
					cropHeight = scaledImageBitmapData.height - 1;
				}
				
				// Create the initial cropping rectangle centered in the display area
				
				cropRect = new Rectangle(cropX, cropY, cropWidth, cropHeight); 
				
				// Draw the initial cropping rectangle
				
				drawCroppingRect();
				
				// Dispatch events if the position, size, and/or aspect ratio of the cropping rectangle was changed
				
				if (!centerCropRect && (cropX != origRect.x || cropY != origRect.y)) dispatchEvent(new Event(CROP_POSITION_CHANGED));
				if (cropWidth != origRect.width || cropHeight != origRect.height) dispatchEvent(new Event(CROP_DIMENSIONS_CHANGED));
				if (cropRatioActive && !isNaN() && cropRatio != origCropRatio) dispatchEvent(new Event(CROP_CONSTRAINT_CHANGED));
			}			
		}
		
		// --------------------------------------------------------------------------------------------------
		// createScaledImage - Create a scaled version of the source image that will fit in the component's display area
		// --------------------------------------------------------------------------------------------------
		
		private function createScaledImage():void {
			
			if (imageBitmapData != null) {
				
				var imageWidth:Number = imageBitmapData.width;
				var imageHeight:Number = imageBitmapData.height;
				
				// Clear the flag that indicates that the image has not been processed yet by the createScaledImage() method
				
				newImageLoaded = false;				
				
				// Initialize the scaling factor to 1 (unscaled)
				
				imageScaleFactor = 1;
				
				// If the image size is larger than the component size
				
				if (imageWidth > componentWidth || imageHeight > componentHeight) {
					
					// Determine the ratio of the size of the loaded image to the component's size
					
					var newXScale:Number = imageWidth == 0 ? 1 : componentWidth / imageWidth;
					var newYScale:Number = imageHeight == 0 ? 1 : componentHeight / imageHeight;
					
					// Calculate the scaling factor based on which dimension must be scaled in order for the image to fit within the component
					
					var x:Number = 0;
					var y:Number = 0;
					
					if (newXScale > newYScale) {
						x = Math.floor((componentWidth - imageWidth * newYScale));
						imageScaleFactor = newYScale;
					}
					else {
						y = Math.floor((componentHeight - imageHeight * newXScale));
						imageScaleFactor = newXScale;
					}
					
					// Create a matrix to perform the image scaling
					
					var scaleMatrix:Matrix = new Matrix();
					scaleMatrix.scale(imageScaleFactor, imageScaleFactor);
					
					// Calculate the scaled size of the image
					
					imageScaledWidth = Math.ceil(imageBitmapData.width * imageScaleFactor);
					imageScaledHeight = Math.ceil(imageBitmapData.height * imageScaleFactor);
					
					// Calculate the new coordinates for the image so that it is centered within the component
					
					imageLocation = new Point(x - ((unscaledWidth - imageScaledWidth) / 2), y - ((unscaledHeight - imageScaledHeight) / 2))			
					
					// If there is a scaled BitmapData object from a previous image, dispose of the data
					
					if (scaledImageBitmapData != null) scaledImageBitmapData.dispose();
					
					// Create a new BitmapData object to hold the scaled image
					
					scaledImageBitmapData = new BitmapData(imageScaledWidth, imageScaledHeight, true, bkgndColor);
					
					// Create the scaled image (use smoothing)
					
					scaledImageBitmapData.draw(imageBitmapData, scaleMatrix, null, null, null, true);
				}
					
					// Else the image size is equal to or smaller than the component size
					
				else {
					
					// The scaled size is the actual size of the image
					
					imageScaledWidth = imageWidth;
					imageScaledHeight = imageHeight;
					
					// Set the new coordinates for the image so that it is centered within the component
					
					imageLocation = new Point((componentWidth - imageWidth) / 2, (componentHeight - imageHeight) / 2);
					
					// The image is unscaled, so just clone the BitmapData
					
					scaledImageBitmapData = imageBitmapData.clone();					
				}
			}			
		}
	}
}