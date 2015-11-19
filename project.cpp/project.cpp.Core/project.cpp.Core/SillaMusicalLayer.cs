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
        CCSprite redChair, blueChair, greenChair, magentaChair, cyanChair, orangeChair;
        List<CCSprite> sillas = new List<CCSprite>();
        int musicId;


        public SillaMusicalLayer() : base(CCColor4B.White)
        {
            redChair = new CCSprite("images/silla_red.png");
            blueChair = new CCSprite("images/silla_blue.png");
            greenChair = new CCSprite("images/silla_green.png");
            magentaChair = new CCSprite("images/silla_magenta.png");
            cyanChair = new CCSprite("images/silla_cyan.png");
            orangeChair = new CCSprite("images/silla_orange.png");

            sillas.Add(redChair);
            sillas.Add(blueChair);
            sillas.Add(greenChair);
            sillas.Add(magentaChair);
            sillas.Add(cyanChair);
            sillas.Add(orangeChair);

            AddChild(redChair);
            AddChild(blueChair);
            AddChild(greenChair);
            AddChild(magentaChair);
            AddChild(cyanChair);
            AddChild(orangeChair);

            CCSimpleAudioEngine.SharedEngine.PreloadEffect("bgm/sillas");
            musicId = CCSimpleAudioEngine.SharedEngine.PlayEffect("bgm/sillas", true);
            CCSimpleAudioEngine.SharedEngine.PauseEffect(musicId);

            // create and initialize a Label
            label = new CCLabel("Silla Musical", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);

            // add the label as a child to this Layer
            AddChild(label);

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
            CCSize tama�o = Scene.Window.WindowSizeInPixels;
            CCPoint centro = tama�o.Center;
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
