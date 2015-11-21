using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;
using System.Threading;
using System.Threading.Tasks;


namespace project.cpp.Core
{
    public class SeleccionJuego : CCLayerColor
    {

        // Define a label variable

        CCLabel label;
        CCLabel sillasLabel;
        CCLabel dictadoLabel;
        string coinsound = "sounds/coin";
        string startsound = "sounds/start";
        string selectsound = "sounds/select";
        int mid;


        public SeleccionJuego() : base(CCColor4B.Red)
        {

            // create and initialize a Label

            label = new CCLabel("Seleccione el juego a jugar!", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);


            // add the label as a child to this Layer
            AddChild(label);
            sillasLabel = new CCLabel("Sillas musicales", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            sillasLabel.Color = CCColor3B.Blue;
            AddChild(sillasLabel);
            dictadoLabel = new CCLabel("Nombre no definido", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            dictadoLabel.Color = CCColor3B.Green;
            AddChild(dictadoLabel);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(startsound);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(coinsound);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(selectsound);


        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;
            // position the label on the center of the screen
            label.Position = bounds.Center;

            //Ubicar las 6 sillas al inicio
            sillasLabel.Position = new CCPoint(400, 250);
            dictadoLabel.Position = new CCPoint(600, 250);


            //TODO hallar el centro de la pantalla   
            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin", false);

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
            if (keyEvent.Keys == CCKeys.D1)
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/start");
                passToGame(1);

            }
            else if (keyEvent.Keys == CCKeys.D2)
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/start");
                passToGame(2);

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

                if (GameData.CheckIfLabelTouched(touch, sillasLabel))
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/start");
                    passToGame(1);

                }
                else if (GameData.CheckIfLabelTouched(touch, dictadoLabel))
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/start");
                    passToGame(2);

                }
                
                else
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin");
                }

                // Perform touch handling here
            }
        }


        public void passToGame(int i)
        {
            GameData.scores = new int[GameData.players];
            CCSimpleAudioEngine.SharedEngine.StopEffect(mid);
            var newScene = new CCScene(Window);
            if(i == 1)
            {
                var silla = new SillaMusicalLayer();
                newScene.AddChild(silla);
                Window.DefaultDirector.ReplaceScene(newScene);

            }
            else
            {
                var dictado = new DictadoLayercs();
                newScene.AddChild(dictado);
                Window.DefaultDirector.ReplaceScene(newScene);

            }

        }
    }
}

