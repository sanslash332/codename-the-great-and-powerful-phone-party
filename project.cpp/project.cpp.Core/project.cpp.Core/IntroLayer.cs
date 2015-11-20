using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;
using System.Threading;
using System.Threading.Tasks;


namespace project.cpp.Core
{
    public class  IntroLayer : CCLayerColor
    {

        // Define a label variable

        CCLabel label;
        CCLabel oneplayerlabel;
        CCLabel twoplayerlabels;
        CCLabel threeplayerlabel;
        CCLabel fourplayerLabel;
        string coinsound = "sounds/coin";
        string startsound = "sounds/start";
        string selectsound = "sounds/select";
        int mid;
                

        public IntroLayer() : base(CCColor4B.Red)
        {

            // create and initialize a Label
            CCSimpleAudioEngine.SharedEngine.PreloadEffect("bgm/title");

            label = new CCLabel("Seleccione cuantos jugadores van a jugar, y luego presione aquí.", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            

            // add the label as a child to this Layer
            AddChild(label);
            oneplayerlabel = new CCLabel("1 jugador", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            oneplayerlabel.Color = CCColor3B.Blue;
            AddChild(oneplayerlabel);
            twoplayerlabels=new CCLabel("2 jugadores", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            twoplayerlabels.Color = CCColor3B.Green;
            AddChild(twoplayerlabels);
            threeplayerlabel= new CCLabel("3 jugadores", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            threeplayerlabel.Color = CCColor3B.Green;
            AddChild(threeplayerlabel);
            fourplayerLabel=new CCLabel("4 jugadores", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            fourplayerLabel.Color = CCColor3B.Green;
            AddChild(fourplayerLabel);

            CCSimpleAudioEngine.SharedEngine.PreloadEffect(startsound);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(coinsound);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(selectsound);


        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;
            mid=CCSimpleAudioEngine.SharedEngine.PlayEffect("bgm/title",true);

            // position the label on the center of the screen
            label.Position = bounds.Center;

            //Ubicar las 6 sillas al inicio
            oneplayerlabel.Position = new CCPoint(400, 250);
            twoplayerlabels.Position = new CCPoint(600, 250);
            threeplayerlabel.Position = new CCPoint(400, 100);
            fourplayerLabel.Position = new CCPoint(600, 100);


            //TODO hallar el centro de la pantalla   
            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin",false);

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            var keyListener = new CCEventListenerKeyboard();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            keyListener.OnKeyPressed = OnKeyPress;
            AddEventListener(touchListener, this);
            AddEventListener(keyListener, this);
        }

        
      void OnKeyPress(CCEventKeyboard keyEvent)
        {
            if(keyEvent.Keys== CCKeys.Enter)
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/start");
                passToGame();
            }
            else if(keyEvent.Keys== CCKeys.D1)
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect(selectsound);
                GameData.players = 1;
                oneplayerlabel.Color = CCColor3B.Blue;
                twoplayerlabels.Color = CCColor3B.Green;
                threeplayerlabel.Color = CCColor3B.Green;
                fourplayerLabel.Color = CCColor3B.Green;

            }
            else if(keyEvent.Keys== CCKeys.D2)
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect(selectsound);
                GameData.players = 2;
                oneplayerlabel.Color = CCColor3B.Green;
                twoplayerlabels.Color = CCColor3B.Blue;
                threeplayerlabel.Color = CCColor3B.Green;
                fourplayerLabel.Color = CCColor3B.Green;

            }

            else if(keyEvent.Keys== CCKeys.D3)
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect(selectsound);
                GameData.players = 3;
                oneplayerlabel.Color = CCColor3B.Green;
                twoplayerlabels.Color = CCColor3B.Green;
                threeplayerlabel.Color = CCColor3B.Blue;
                fourplayerLabel.Color = CCColor3B.Green;


            }
            else if(keyEvent.Keys== CCKeys.D4)
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect(selectsound);
                GameData.players = 4;
                oneplayerlabel.Color = CCColor3B.Green;
                twoplayerlabels.Color = CCColor3B.Green;
                threeplayerlabel.Color = CCColor3B.Green;
                fourplayerLabel.Color = CCColor3B.Blue;

            }
            else
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin");
                
            }
                

        }

        
        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                CCTouch touch = touches[0];
                
                if (GameData.CheckIfLabelTouched(touch, label))
                {


                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/start");
                    passToGame();
                }
                else if (GameData.CheckIfLabelTouched(touch, oneplayerlabel))
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect(selectsound);
                    GameData.players = 1;
                    oneplayerlabel.Color = CCColor3B.Blue;
                    twoplayerlabels.Color = CCColor3B.Green;
                    threeplayerlabel.Color = CCColor3B.Green;
                    fourplayerLabel.Color = CCColor3B.Green;

                }
                else if (GameData.CheckIfLabelTouched(touch, twoplayerlabels))
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect(selectsound);
                    GameData.players = 2;
                    oneplayerlabel.Color = CCColor3B.Green;
                    twoplayerlabels.Color = CCColor3B.Blue;
                    threeplayerlabel.Color = CCColor3B.Green;
                    fourplayerLabel.Color = CCColor3B.Green;

                }
                else if (GameData.CheckIfLabelTouched(touch, threeplayerlabel))
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect(selectsound);
                    GameData.players= 3;
                    oneplayerlabel.Color = CCColor3B.Green;
                    twoplayerlabels.Color = CCColor3B.Green;
                    threeplayerlabel.Color = CCColor3B.Blue;
                    fourplayerLabel.Color = CCColor3B.Green;


                }
                else if (GameData.CheckIfLabelTouched(touch, fourplayerLabel))
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect(selectsound);
                    GameData.players = 4;
                    oneplayerlabel.Color = CCColor3B.Green;
                    twoplayerlabels.Color = CCColor3B.Green;
                    threeplayerlabel.Color = CCColor3B.Green;
                    fourplayerLabel.Color = CCColor3B.Blue;

                }
                else
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin");
                }

                // Perform touch handling here
            }
        }


        public void passToGame()
        {
            GameData.scores = new int[GameData.players];
            CCSimpleAudioEngine.SharedEngine.StopEffect(mid);
            var newScene = new CCScene(Window);
            var silla = new SillaMusicalLayer();
            newScene.AddChild(silla);
            Window.DefaultDirector.ReplaceScene(newScene);
            
        }
    }
}   

