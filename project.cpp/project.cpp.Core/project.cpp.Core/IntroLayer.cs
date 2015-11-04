using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;

namespace project.cpp.Core
{
    public class  IntroLayer : CCLayerColor
    {

        // Define a label variable
        CCLabel label;
        int sid;
        

        public IntroLayer() : base(CCColor4B.Red)
        {

            // create and initialize a Label
            label = new CCLabel("Toca la pantalla, presiona start, o presiona enter para iniciar.", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);

            // add the label as a child to this Layer
            AddChild(label);
                CCSimpleAudioEngine.SharedEngine.PreloadEffect("sounds/start");
            CCSimpleAudioEngine.SharedEngine.PreloadEffect("sounds/coin");

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            label.Position = bounds.Center;

            //Ubicar las 6 sillas al inicio
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
            else
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin");
            }
                

        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/start");
                passToGame();
                // Perform touch handling here
            }
        }


        public void passToGame()
        {
            var newScene = new CCScene(Window);
            var silla = new SillaMusicalLayer();
            newScene.AddChild(silla);
            Window.DefaultDirector.ReplaceScene(newScene);
            



        }
    }
}   

