using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;

namespace project.cpp.Core
{
    public class SillaMusicalLayer : CCLayerColor
    {

        // Define a label variable
        CCLabel label;
        CCSprite redChair, greenChair, blueChair,  magentaChair, cyanChair, orangeChair;
        List<CCSprite> sillas = new List<CCSprite>();
        int musicId;
        string soundJump = "sounds/jump";
        string soundFall = "sounds/fall";
        string soundSit = "sounds/sit";
        string soundChooce = "sounds/chooce";
         bool started;
        int rownd;
        CCLabel p1button, p2button, p3button, p4button;
        List<bool> buttonIsBlocked;
        int timePassed;
        bool inmusic;



        public SillaMusicalLayer() : base(CCColor4B.White)
        {
            redChair = new CCSprite("images/silla_red.png");
            blueChair = new CCSprite("images/silla_blue.png");
            greenChair = new CCSprite("images/silla_green.png");
            magentaChair = new CCSprite("images/silla_magenta.png");
            cyanChair = new CCSprite("images/silla_cyan.png");
            orangeChair = new CCSprite("images/silla_orange.png");

            p1button = new CCLabel("p1", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            p2button = new CCLabel("p2", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            p3button = new CCLabel("p3", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            p4button = new CCLabel("p4", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            label = new CCLabel("bienvenido a la cilla musical. El último jugador en presionar su botón cuando la música pare, perderá. ¡ojo! No presiones si la música aún suena, te irá mal. Toque para iniciar.", "font/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            AddChild(label);
            rownd = 0;
            started = false;
            inmusic = false;
            timePassed = 0;
            buttonIsBlocked = new List<bool>();
            
            

            
            sillas.Add(redChair);
            buttonIsBlocked.Add(false);
            AddChild(p1button);
            if (GameData.players>=2)
            {

                sillas.Add(greenChair);
                buttonIsBlocked.Add(false);
                AddChild(p2button);

            }
            if(GameData.players>=3)
            {
                sillas.Add(blueChair);
                AddChild(p3button);
                buttonIsBlocked.Add(false);
            }
            if(GameData.players>=4)
            {
                sillas.Add(orangeChair);
                AddChild(p4button);
                buttonIsBlocked.Add(false);
            }

            
            //sillas.Add(magentaChair);
            //sillas.Add(cyanChair);
            

            //AddChild(redChair);
            //AddChild(blueChair);
            //AddChild(greenChair);
            //AddChild(magentaChair);
            //AddChild(cyanChair);
            //AddChild(orangeChair);

            CCSimpleAudioEngine.SharedEngine.PreloadEffect("bgm/sillas");
            musicId = CCSimpleAudioEngine.SharedEngine.PlayEffect("bgm/sillas", true);
            CCSimpleAudioEngine.SharedEngine.PauseEffect(musicId);

            // create and initialize a Label
            


        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCSimpleAudioEngine.SharedEngine.ResumeAllEffects();
            

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            label.Position = bounds.LowerLeft;

            //Ubicar las 6 sillas al inicio
            //TODO hallar el centro de la pantalla   
            CCSize tamaño = Scene.Window.WindowSizeInPixels;
            CCPoint centro = tamaño.Center;
            double cx = centro.X;
            double cy = centro.Y;
            double radio = 200;

            for (int i = 0; i < sillas.Count; i++)
            {
                double xpos = cx + radio * Math.Sin(2 * Math.PI / 6 * i);
                double ypos = cy + radio * Math.Cos(2 * Math.PI / 6 * i);
                CCPoint position = new CCPoint((float)xpos, (float)ypos);
                sillas[i].Position = position;
                sillas[i].Rotation = (float)(180 + 360 / 6 * i);
            }


            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }
    }
}

