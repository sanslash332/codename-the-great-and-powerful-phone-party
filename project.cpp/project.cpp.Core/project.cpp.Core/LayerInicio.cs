using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;
using System.Threading;
using System.Threading.Tasks;


namespace project.cpp.Core
{
    public class LayerInicio : CCLayerColor
    {
        CCSprite fondo;
        CCLabel label;
        string coinsound = "sounds/coin";
        string startsound = "sounds/start";
        string selectsound = "sounds/select";
        int mid;


        public LayerInicio() : base(CCColor4B.Red)
        {

            // create and initialize a Label
            CCSimpleAudioEngine.SharedEngine.PreloadEffect("bgm/title");
            AgregarFondo();
            label = new CCLabel("Presione cualquier cosa para continuar", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            AddChild(label);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(startsound);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(coinsound);
            CCSimpleAudioEngine.SharedEngine.PreloadEffect(selectsound);


        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;
            mid = CCSimpleAudioEngine.SharedEngine.PlayEffect("bgm/title", true);
            fondo.Position = bounds.Center;
            label.Position = bounds.Center;

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
            passToGame();
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            passToGame();
        }
        private void AgregarFondo()
        {
            fondo = new CCSprite("images/inicio_pc");
            AddChild(fondo);
        }


        public void passToGame()
        {
            GameData.scores = new int[GameData.players];
            CCSimpleAudioEngine.SharedEngine.StopEffect(mid);
            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin");
            var newScene = new CCScene(Window);
            var silla = new IntroLayer();
            newScene.AddChild(silla);
            Window.DefaultDirector.ReplaceScene(newScene);

        }
    }
}

