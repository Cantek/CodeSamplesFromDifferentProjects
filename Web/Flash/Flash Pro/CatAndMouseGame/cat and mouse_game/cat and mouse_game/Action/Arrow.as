package 
{
    import flash.display.*;
    import flash.events.*;

    public class Arrow extends MovieClip
    {
        public var direction:String;
        public var index:int = -1;
        private var spacing:int;
        private var numColumns:int = 15;

        public function Arrow(param1, param2, param3) : void
        {
            addFrameScript(0, this.frame1);
            this.direction = param1;
            this.x = param2;
            this.y = param3;
            this.gotoAndStop(param1);
            this.addEventListener(MouseEvent.MOUSE_DOWN, this.arrowDrag);
            this.addEventListener(MouseEvent.MOUSE_UP, this.arrowDrop);
            this.spacing = this.width;
            return;
        }// end function

        private function arrowDrag(event:MouseEvent)
        {
            event.target.startDrag();
            return;
        }// end function

        private function arrowDrop(event:MouseEvent)
        {
            event.target.stopDrag();
            var _loc_2:* = Math.floor((event.target.x + this.spacing / 2) / this.spacing);
            var _loc_3:* = Math.floor((event.target.y + this.spacing / 2) / this.spacing);
            this.x = _loc_2 * this.spacing;
            this.y = _loc_3 * this.spacing;
            this.index = _loc_3 * this.numColumns + _loc_2;
            return;
        }// end function

        function frame1()
        {
            stop();
            return;
        }// end function

    }
}
