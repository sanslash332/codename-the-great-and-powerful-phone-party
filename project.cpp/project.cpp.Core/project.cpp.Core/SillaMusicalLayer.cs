using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;
using System.Threading.Tasks;

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
        string soundGo = "sounds/go";
        string soundMSG = "sounds/msgappear";
        string soundChooce = "sounds/chooce";
         bool started;
        int rownd;
        CCLabel p1button, p2button, p3button, p4button;
        List<bool> buttonIsBlocked;
        int timePassed;
        bool inmusic;
        bool inSplash;
        List<CCLabel> buttons;


        public SillaMusicalLayer() : base(CCColor4B.White)
        {
            redChair = new CCSprite("images/silla_red.png");
            blueChair = new CCSprite("images/silla_blue.png");
            greenChair = new CCSprite("images/silla_green.png");
            magentaChair = new CCSprite("images/silla_magenta.png");
            cyanChair = new CCSprite("images/silla_cyan.png");
            orangeChair = new CCSprite("images/silla_orange.png");

            p1button = new CCLabel("p1", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            p1button.Color = CCColor3B.Blue;
            p2button = new CCLabel("p2", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            p2button.Color = CCColor3B.Red;
            p3button = new CCLabel("p3", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            p3button.Color = CCColor3B.Green;
            p4button = new CCLabel("p4", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            p4button.Color = CCColor3B.Orange;
            buttons = new List<CCLabel>();

            label = new CCLabel("bienvenido a la cilla musical. El último jugador en presionar su botón cuando la música pare, perderá. ¡ojo! No presiones si la música aún suena, te irá mal. Toque para iniciar.", "font/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            AddChild(label);
            rownd = 0;
            started = false;
            inmusic = false;
            timePassed = 0;
            inSplash = false;

            buttonIsBlocked = new List<bool>();
          
            sillas.Add(redChair);
            buttonIsBlocked.Add(false);
            buttons.Add(p1button);
            AddChild(p1button);
            if (GameData.players>=2)
            {

                sillas.Add(greenChair);
                buttonIsBlocked.Add(false);
                AddChild(p2button);
                buttons.Add(p2button);

            }
            if(GameData.players>=3)
            {
                sillas.Add(blueChair);
                AddChild(p3button);
                buttonIsBlocked.Add(false);
                buttons.Add(p3button);
            }
            if(GameData.players>=4)
            {
                sillas.Add(orangeChair);
                AddChild(p4button);
                buttonIsBlocked.Add(false);
                buttons.Add(p4button);
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

        void addCillas()
        {

            //Ubicar las 6 sillas al inicio
            //TODO hallar el centro de la pantalla   
            CCSize tamaño = Scene.Window.WindowSizeInPixels;
            CCPoint centro = tamaño.Center;
            double cx = centro.X;
            double cy = centro.Y;
            double radio = 200;

            for (int i = 0; i < sillas.Count; i++)
            {
                sillas[i].RemoveFromParent();

                double xpos = cx + radio * Math.Sin(2 * Math.PI / 4 * i);
                double ypos = cy + radio * Math.Cos(2 * Math.PI / 4 * i);
                CCPoint position = new CCPoint((float)xpos, (float)ypos);
                AddChild(sillas[i]);
                sillas[i].Position = position;
                sillas[i].Rotation = (float)(180 + 360 / 6 * i);
            }



        }
        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCSimpleAudioEngine.SharedEngine.PlayEffect(soundMSG);
            

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            label.Position = bounds.Center;

            p1button.Position = new CCPoint(10, 10);
            p2button.Position = new CCPoint(200, 10);
            p3button.Position = new CCPoint(10, 250);
            p4button.Position = new CCPoint(200, 250);


            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            CCEventListenerKeyboard keyboardListener = new CCEventListenerKeyboard();

            keyboardListener.OnKeyPressed = onKeyPress;
            touchListener.OnTouchesEnded = OnTouchesEnded;

            AddEventListener(touchListener, this);
            AddEventListener(keyboardListener, this);

        }

        async void prepareRown()
        {
            inSplash = true;
             
            label.Text = "atentos a la música!";
            addCillas();
            await Task.Delay(500);
            CCSimpleAudioEngine.SharedEngine.PlayEffect(soundGo);

            await Task.Delay(200);
            label.RemoveFromParent();
            inmusic = true;
            inSplash = false;
            timePassed = 0;


            CCSimpleAudioEngine.SharedEngine.ResumeEffect(musicId);



        }

        void passTheTime(float frametime)
        {
            timePassed++;
            if (inmusic && timePassed >= 400)
            {
                Random ropo = new Random();
                int prob = ropo.Next(1, 100);
                if (prob >= 75)
                {
                    inmusic = false;
                    CCSimpleAudioEngine.SharedEngine.PauseEffect(musicId);
                    timePassed = 0;
                }

            }


            }

        


        void endRownd()
        {

        }

        void setStart()
        {
            started = true;
            rownd = 1;

            CCSimpleAudioEngine.SharedEngine.PlayEffect(soundChooce);
            Schedule(passTheTime);
            prepareRown();
          

        }
        void onKeyPress(CCEventKeyboard keyEvent)
        {
            if(!started&&keyEvent.Keys== CCKeys.Enter)
            {
                setStart();


            }

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

