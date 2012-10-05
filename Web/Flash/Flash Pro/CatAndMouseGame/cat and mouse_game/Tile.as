package 
{
    import flash.display.*;

    public class Tile extends MovieClip
    {
        public var index:int;
        public var type:String;

        public function Tile(param1:int) : void
        {
            addFrameScript(0, this.frame1, 89, this.frame90);
            this.index = param1;
            this.setType("normal");
            return;
        }// end function

        public function setType(param1:String)
        {
            this.type = param1;
            if (param1 == "normal")
            {
                if (this.index % 2 == 0)
                {
                    param1 = "even";
                }
                else
                {
                    param1 = "odd";
                }
            }
            this.gotoAndStop(param1);
            return;
        }// end function

        function frame1()
        {
            stop();
            return;
        }// end function

        function frame90()
        {
            stop();
            return;
        }// end function

    }
}
