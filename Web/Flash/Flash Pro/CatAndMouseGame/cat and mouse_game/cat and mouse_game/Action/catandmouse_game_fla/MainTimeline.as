package catandmouse_game_fla
{
    import fl.transitions.*;
    import fl.transitions.easing.*;
    import flash.display.*;
    import flash.events.*;
    import flash.media.*;
    import flash.utils.*;

    dynamic public class MainTimeline extends MovieClip
    {
        public var bSound:SimpleButton;
        public var bRun:SimpleButton;
        public var level1:Array;
        public var level2:Array;
        public var level3:Array;
        public var aTileData:Array;
        public var mapGrid:Sprite;
        public var numRows:int;
        public var numColumns:int;
        public var numTiles:int;
        public var spacing:int;
        public var currentColumn:int;
        public var currentRow:int;
        public var aTileMap:Array;
        public var aTypes:Array;
        public var hasKey:Boolean;
        public var iconMouse:realMouseMovie;
        public var arrowUp:Arrow;
        public var arrowDown:Arrow;
        public var arrowLeft:Arrow;
        public var arrowRight:Arrow;
        public var gameoverWithCat:catSound;
        public var gameoverWithCatcher:catcherSound;
        public var gameoverWithWall:wallSound;
        public var gameWinSound:winSound;
        public var gameSound:bgSound;
        public var nextLevelSound:levelSound;
        public var keyFoundSound:keySound;
        public var keyFoundGateSound:gateSound;
        public var intro:introMovie;
        public var gameOver:gameOverMovie;
        public var gameWin:gameWinMovie;
        public var levelT:levelTwo;
        public var levelTh:levelThree;
        public var levelNum:int;
        public var sound:Boolean;
        public var nextX:int;
        public var nextY:int;
        public var numNext:Object;
        public var timer:Timer;

        public function MainTimeline()
        {
            addFrameScript(0, this.frame1);
            return;
        }// end function

        public function deleteIntro(event:MouseEvent)
        {
            removeChild(this.intro);
            return;
        }// end function

        public function deleteGameOver(event:MouseEvent)
        {
            removeChild(this.gameOver);
            return;
        }// end function

        public function deleteGameWin(event:MouseEvent)
        {
            removeChild(this.gameWin);
            return;
        }// end function

        public function deleteLevelT(event:MouseEvent)
        {
            removeChild(this.levelT);
            return;
        }// end function

        public function deleteLevelTh(event:MouseEvent)
        {
            removeChild(this.levelTh);
            return;
        }// end function

        public function nextLevel(param1:int)
        {
            this.nextX = this.spacing;
            this.nextY = 0;
            switch(param1)
            {
                case 1:
                {
                    addChild(this.levelT);
                    this.levelT.play();
                    break;
                }
                case 2:
                {
                    addChild(this.levelTh);
                    this.levelTh.play();
                    break;
                }
                case 13:
                {
                    addChild(this.gameOver);
                    this.gameOver.play();
                    break;
                }
                default:
                {
                    addChild(this.gameWin);
                    this.gameWin.play();
                    break;
                    break;
                }
            }
            return;
        }// end function

        public function cleanAndRecreate()
        {
            this.mapGrid = new Sprite();
            this.arrowUp = new Arrow("up", 630, 100);
            this.arrowDown = new Arrow("down", 630, 150);
            this.arrowLeft = new Arrow("left", 630, 200);
            this.arrowRight = new Arrow("right", 630, 250);
            this.iconMouse = new realMouseMovie();
            this.iconMouse.x = -2 * this.spacing;
            this.iconMouse.y = 0 * this.spacing;
            this.hasKey = false;
            return;
        }// end function

        public function drawTiles()
        {
            this.mapGrid.x = 110;
            this.mapGrid.y = 40;
            addChild(this.mapGrid);
            this.currentColumn = 0;
            this.currentRow = 0;
            this.aTileMap = new Array();
            var _loc_1:int = 0;
            while (_loc_1 < this.numTiles)
            {
                
                if (this.currentColumn == this.numColumns)
                {
                    (this.currentRow + 1);
                    this.currentColumn = 0;
                }
                this.aTileMap[_loc_1] = new Tile(_loc_1);
                this.aTileMap[_loc_1].x = this.currentColumn * this.spacing;
                this.aTileMap[_loc_1].y = this.currentRow * this.spacing;
                this.aTileMap[_loc_1].setType(this.aTileData[_loc_1]);
                this.mapGrid.addChild(this.aTileMap[_loc_1]);
                (this.currentColumn + 1);
                _loc_1++;
            }
            this.mapGrid.addChild(this.arrowUp);
            this.mapGrid.addChild(this.arrowDown);
            this.mapGrid.addChild(this.arrowLeft);
            this.mapGrid.addChild(this.arrowRight);
            this.mapGrid.addChild(this.iconMouse);
            this.iconMouse.x = -2 * this.spacing;
            this.iconMouse.y = 0 * this.spacing;
            return;
        }// end function

        public function showIntro()
        {
            this.gameSound.play();
            this.drawTiles();
            addChild(this.intro);
            return;
        }// end function

        public function run(event:MouseEvent) : void
        {
            this.runNext();
            return;
        }// end function

        public function soundOnOff(event:MouseEvent) : void
        {
            if (this.sound)
            {
                SoundMixer.stopAll();
            }
            else
            {
                this.gameSound.play();
            }
            this.sound = !this.sound;
            return;
        }// end function

        public function runNext() : void
        {
            var _loc_1:* = undefined;
            var _loc_2:Tween = null;
            var _loc_3:* = undefined;
            var _loc_4:Tween = null;
            if (this.nextX != 0)
            {
                _loc_1 = this.iconMouse.x + this.nextX;
                _loc_2 = new Tween(this.iconMouse, "x", None.easeNone, this.iconMouse.x, _loc_1, 0.5, true);
                _loc_2.addEventListener(TweenEvent.MOTION_FINISH, this.checkNextSquare);
            }
            if (this.nextY != 0)
            {
                _loc_3 = this.iconMouse.y + this.nextY;
                _loc_4 = new Tween(this.iconMouse, "y", None.easeNone, this.iconMouse.y, _loc_3, 0.5, true);
                _loc_4.addEventListener(TweenEvent.MOTION_FINISH, this.checkNextSquare);
            }
            return;
        }// end function

        public function checkNextSquare(event:TweenEvent) : void
        {
            var _loc_4:int = 0;
            var _loc_5:uint = 0;
            var _loc_2:* = Math.floor((this.iconMouse.x + this.nextX) / this.spacing);
            var _loc_3:* = Math.floor((this.iconMouse.y + this.nextY) / this.spacing);
            if (_loc_3 >= 0 && _loc_3 < this.numRows && _loc_2 >= 0 && _loc_2 < this.numColumns)
            {
                _loc_4 = _loc_3 * this.numColumns + _loc_2;
                if (this.aTileMap[_loc_4].type == "normal")
                {
                    this.runNext();
                }
                else if (this.aTileMap[_loc_4].type == "key")
                {
                    this.hasKey = true;
                    this.keyFoundSound.play();
                    this.aTileMap[_loc_4].setType("normal");
                    this.runNext();
                }
                else if (this.aTileMap[_loc_4].type == "gate")
                {
                    if (this.hasKey)
                    {
                        this.keyFoundGateSound.play();
                        this.runNext();
                        this.aTileMap[_loc_4].setType("normal");
                    }
                    else
                    {
                        this.gameoverWithWall.play();
                        removeChild(this.mapGrid);
                        this.aTileData = this.aTileData;
                        this.levelNum = this.levelNum;
                        this.cleanAndRecreate();
                        this.drawTiles();
                        this.nextLevel(13);
                    }
                }
                else if (this.aTileMap[_loc_4].type == "mouseCatcher" || this.aTileMap[_loc_4].type == "cat" || this.aTileMap[_loc_4].type == "wall")
                {
                    removeChild(this.mapGrid);
                    if (this.aTileMap[_loc_4].type == "wall")
                    {
                        this.gameoverWithWall.play();
                        this.aTileData = this.aTileData;
                        this.levelNum = this.levelNum;
                    }
                    else
                    {
                        if (this.aTileMap[_loc_4].type == "cat")
                        {
                            this.gameoverWithCat.play();
                        }
                        if (this.aTileMap[_loc_4].type == "mouseCatcher")
                        {
                            this.gameoverWithCatcher.play();
                        }
                        this.aTileData = this.level1;
                        this.levelNum = 0;
                    }
                    this.cleanAndRecreate();
                    this.drawTiles();
                    this.nextLevel(13);
                }
                else if (this.aTileMap[_loc_4].type == "treasure")
                {
                    this.aTileMap[_loc_4].setType("win");
                    if (this.levelNum >= 0 && this.levelNum < 2)
                    {
                        this.nextLevelSound.play();
                    }
                    else
                    {
                        this.gameWinSound.play();
                    }
                    _loc_5 = this.mapGrid.numChildren - 1;
                    this.mapGrid.setChildIndex(this.aTileMap[_loc_4], _loc_5);
                    this.timer.addEventListener(TimerEvent.TIMER_COMPLETE, this.myTimerF);
                    this.timer.start();
                }
                if (this.arrowUp.index == _loc_4)
                {
                    this.nextX = 0;
                    this.nextY = -this.spacing;
                }
                if (this.arrowDown.index == _loc_4)
                {
                    this.nextX = 0;
                    this.nextY = this.spacing;
                }
                if (this.arrowLeft.index == _loc_4)
                {
                    this.nextX = -this.spacing;
                    this.nextY = 0;
                }
                if (this.arrowRight.index == _loc_4)
                {
                    this.nextX = this.spacing;
                    this.nextY = 0;
                }
            }
            else
            {
                this.gameoverWithWall.play();
                removeChild(this.mapGrid);
                this.aTileData = this.aTileData;
                this.levelNum = this.levelNum;
                this.cleanAndRecreate();
                this.drawTiles();
                this.nextLevel(13);
            }
            return;
        }// end function

        public function myTimerF(event:TimerEvent)
        {
            removeChild(this.mapGrid);
            var _loc_2:String = this;
            var _loc_3:* = this.levelNum + 1;
            _loc_2.levelNum = _loc_3;
            if (this.levelNum == 1)
            {
                this.aTileData = this.level2;
            }
            else if (this.levelNum == 2)
            {
                this.aTileData = this.level3;
            }
            else
            {
                this.aTileData = this.level1;
                this.levelNum = 0;
            }
            this.cleanAndRecreate();
            this.drawTiles();
            this.nextLevel(this.levelNum);
            return;
        }// end function

        function frame1()
        {
            this.level1 = new Array("normal", "normal", "normal", "wall", "normal", "normal", "normal", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "normal", "treasure", "normal", "gate", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "normal", "normal", "normal", "wall", "mouseCatcher", "normal", "normal", "normal", "wall", "normal", "normal", "normal", "normal", "normal", "wall", "wall", "wall", "wall", "wall", "normal", "normal", "normal", "wall", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "wall", "normal", "normal", "wall", "wall", "normal", "mouseCatcher", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "wall", "normal", "normal", "normal", "normal", "wall", "wall", "wall", "wall", "normal", "normal", "normal", "normal", "wall", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "key", "normal", "normal", "mouseCatcher", "wall", "wall", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "normal", "normal", "normal", "normal", "normal", "wall", "normal", "normal", "normal", "normal", "normal");
            this.level2 = new Array("normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "cat", "normal", "wall", "wall", "wall", "wall", "wall", "wall", "wall", "wall", "normal", "wall", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "wall", "wall", "wall", "normal", "normal", "cat", "normal", "normal", "normal", "wall", "wall", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "normal", "normal", "normal", "normal", "wall", "wall", "wall", "wall", "normal", "normal", "normal", "normal", "cat", "normal", "wall", "normal", "wall", "normal", "wall", "wall", "normal", "normal", "wall", "normal", "normal", "wall", "wall", "wall", "wall", "wall", "normal", "wall", "normal", "normal", "key", "normal", "mouseCatcher", "wall", "normal", "normal", "wall", "normal", "normal", "wall", "normal", "normal", "normal", "normal", "normal", "wall", "wall", "wall", "wall", "normal", "normal", "wall", "normal", "treasure", "wall", "normal", "normal", "normal", "normal", "normal", "wall", "normal", "normal", "normal", "normal", "normal", "gate", "normal", "normal", "gate", "normal", "normal", "normal", "normal", "normal", "wall", "normal", "mouseCatcher", "normal", "normal");
            this.level3 = new Array("normal", "normal", "wall", "normal", "normal", "cat", "normal", "wall", "normal", "normal", "normal", "normal", "normal", "key", "normal", "normal", "mouseCatcher", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "cat", "normal", "normal", "wall", "wall", "normal", "normal", "wall", "wall", "normal", "wall", "wall", "wall", "normal", "normal", "wall", "wall", "wall", "wall", "normal", "cat", "normal", "wall", "normal", "normal", "wall", "normal", "normal", "cat", "normal", "normal", "normal", "normal", "normal", "cat", "normal", "normal", "wall", "normal", "normal", "wall", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "normal", "normal", "wall", "normal", "wall", "wall", "gate", "wall", "wall", "wall", "wall", "wall", "wall", "normal", "wall", "normal", "wall", "wall", "normal", "normal", "wall", "normal", "normal", "normal", "wall", "normal", "mouseCatcher", "normal", "normal", "wall", "normal", "wall", "normal", "normal", "normal", "wall", "normal", "normal", "normal", "wall", "treasure", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "normal", "wall", "wall", "wall", "normal", "wall", "wall", "normal", "normal", "mouseCatcher", "normal", "normal", "normal", "normal", "normal", "normal", "cat", "normal", "normal", "normal", "normal", "normal", "normal", "normal");
            this.aTileData = this.level1;
            this.mapGrid = new Sprite();
            this.numRows = 10;
            this.numColumns = 15;
            this.numTiles = this.numRows * this.numColumns;
            this.spacing = 40;
            this.currentColumn = 0;
            this.currentRow = 0;
            this.aTileMap = new Array();
            this.aTypes = new Array("normal", "cat", "wall", "gate", "key", "treasure", "mouseCatcher");
            this.hasKey = false;
            this.iconMouse = new realMouseMovie();
            this.arrowUp = new Arrow("up", 638, 100);
            this.arrowDown = new Arrow("down", 638, 150);
            this.arrowLeft = new Arrow("left", 638, 200);
            this.arrowRight = new Arrow("right", 638, 250);
            this.gameoverWithCat = new catSound();
            this.gameoverWithCatcher = new catcherSound();
            this.gameoverWithWall = new wallSound();
            this.gameWinSound = new winSound();
            this.gameSound = new bgSound();
            this.nextLevelSound = new levelSound();
            this.keyFoundSound = new keySound();
            this.keyFoundGateSound = new gateSound();
            this.intro = new introMovie();
            this.gameOver = new gameOverMovie();
            this.gameWin = new gameWinMovie();
            this.levelT = new levelTwo();
            this.levelTh = new levelThree();
            this.intro.addEventListener(MouseEvent.CLICK, this.deleteIntro);
            this.gameOver.addEventListener(MouseEvent.CLICK, this.deleteGameOver);
            this.gameWin.addEventListener(MouseEvent.CLICK, this.deleteGameWin);
            this.levelT.addEventListener(MouseEvent.CLICK, this.deleteLevelT);
            this.levelTh.addEventListener(MouseEvent.CLICK, this.deleteLevelTh);
            this.showIntro();
            this.levelNum = 0;
            this.sound = true;
            this.bRun.addEventListener(MouseEvent.CLICK, this.run);
            this.bSound.addEventListener(MouseEvent.CLICK, this.soundOnOff);
            this.nextX = this.spacing;
            this.nextY = 0;
            this.numNext = 0;
            this.timer = new Timer(2000, 1);
            return;
        }// end function

    }
}
