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
        CCSprite p1button, p2button, p3button, p4button;
        CCSprite fondo;
        List<bool> buttonIsBlocked;
        List<bool> playerIsSat;
        int timePassed;
        int lastPlayerPressed;
        bool inmusic;
        bool inSplash;
        List<CCSprite> buttons;
        List<bool> playerIsRetired;
        bool end = false;
        int perdedores = 0;


        public SillaMusicalLayer() : base(CCColor4B.White)
        {
            perdedores = 0;
            GameData.orden = new int[4];
            AgregarFondo();
            redChair = new CCSprite("images/silla_red.png");
            blueChair = new CCSprite("images/silla_blue.png");
            greenChair = new CCSprite("images/silla_green.png");
            magentaChair = new CCSprite("images/silla_magenta.png");
            cyanChair = new CCSprite("images/silla_cyan.png");
            orangeChair = new CCSprite("images/silla_orange.png");

            p1button = new CCSprite("images/p1_logo");
            p2button = new CCSprite("images/p2_logo");
            p3button = new CCSprite("images/p3_logo");
            p4button = new CCSprite("images/p4_logo");


            buttons = new List<CCSprite>();
            playerIsSat = new List<bool>();

            label = new CCLabel("bienvenido a la silla musical. El último jugador en presionar su botón cuando la música pare, perderá. \n ¡ojo! No presiones si la música aún suena, te irá mal.\n Toque para iniciar.", "fonts/MarkerFelt", 16, CCLabelFormat.SystemFont);
            AddChild(label);
            rownd = 0;
            lastPlayerPressed = -1;
            started = false;
            inmusic = false;
            timePassed = 0;
            inSplash = false;

            buttonIsBlocked = new List<bool>();
            playerIsRetired = new List<bool>();
            sillas.Add(redChair);
            buttonIsBlocked.Add(false);
            playerIsSat.Add(false);
            playerIsRetired.Add(false);
            buttons.Add(p1button);
            AddChild(p1button);
            if (GameData.players>=2)
            {

                sillas.Add(greenChair);
                buttonIsBlocked.Add(false);
                playerIsRetired.Add(false);
                playerIsSat.Add(false);
                AddChild(p2button);
                buttons.Add(p2button);

            }
            if(GameData.players>=3)
            {
                sillas.Add(blueChair);
                AddChild(p3button);
                playerIsSat.Add(false);
                playerIsRetired.Add(false);
                buttonIsBlocked.Add(false);
                buttons.Add(p3button);
            }
            if(GameData.players>=4)
            {
                sillas.Add(orangeChair);
                AddChild(p4button);
                playerIsRetired.Add(false);
                buttonIsBlocked.Add(false);
                buttons.Add(p4button);
                playerIsSat.Add(false);
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
                if(playerIsRetired[i])
                {
                    continue;
                }

                double xpos = cx + radio * Math.Sin(2 * Math.PI / 4 * i);
                double ypos = cy + radio * Math.Cos(2 * Math.PI / 4 * i);
                CCPoint position = new CCPoint((float)xpos, (float)ypos);
                AddChild(sillas[i]);
                sillas[i].Position = position;
                sillas[i].Rotation = 90 * i;
            }




        }
        protected override void AddedToScene()
        {
            base.AddedToScene();
            CCSize tamaño = Scene.Window.WindowSizeInPixels;
            CCSimpleAudioEngine.SharedEngine.StopEffect(musicId);

            CCSimpleAudioEngine.SharedEngine.PlayEffect(soundMSG);
            

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;
            fondo.Position = bounds.Center;

            // position the label on the center of the screen

            label.Color = CCColor3B.Black;
            label.Position = bounds.Center;

            p1button.Position = GetPosicionJugador(1);
            p2button.Position = GetPosicionJugador(2);
            p3button.Position = GetPosicionJugador(3);
            p4button.Position = GetPosicionJugador(4);

            GameData.ResizeBackground(fondo, this);

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            CCEventListenerKeyboard keyboardListener = new CCEventListenerKeyboard();

            keyboardListener.OnKeyPressed = onKeyPress;
            touchListener.OnTouchesEnded = OnTouchesEnded;

            AddEventListener(touchListener, this);
            AddEventListener(keyboardListener, this);

        }

        private void AgregarFondo()
        {
            fondo = new CCSprite("images/silla_pc");
            AddChild(fondo);
        }

        async void prepareRown()
        {
            inSplash = true;
            CCSimpleAudioEngine.SharedEngine.PlayEffect(soundMSG);

            label.RemoveFromParent();
            label.Text = "atentos a la música! Iniciando la ronda" + rownd.ToString() +  "!";
            AddChild(label);
            
            await Task.Delay(1000);
            CCSimpleAudioEngine.SharedEngine.PlayEffect(soundGo);

            
            label.RemoveFromParent();
            addCillas();
            await Task.Delay(100);
            inmusic = true;
            inSplash = false;
            timePassed = 0;


            CCSimpleAudioEngine.SharedEngine.ResumeEffect(musicId);

            lastPlayerPressed = -1;

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
            else if(!inmusic&&!inSplash&&timePassed>=120)
            {
                
                inSplash = true;


                endRownd();
            }

            }

        async void processPress(int playerPressed)
        {
            if(inmusic)
            {
                if (!buttonIsBlocked[playerPressed]&&!playerIsRetired[playerPressed])
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect(soundFall);
                    buttons[playerPressed].Color = CCColor3B.Black;

                    buttonIsBlocked[playerPressed] = true;
                    
                    //lastPlayerPressed = playerPressed;


                }
            }
            else if(!inmusic&&!inSplash)
            {
                if(!playerIsSat[playerPressed] && !buttonIsBlocked[playerPressed]&&!playerIsRetired[playerPressed])
                {
                    CCSimpleAudioEngine.SharedEngine.PlayEffect(soundJump);
                    bool cansit = false;
                    for(int i = 0; i<playerIsSat.Count;i++)
                    {
                        if(i== playerPressed||playerIsRetired[i])
                        {
                            continue;
                        }
                        else
                        {
                            if(!playerIsSat[i])
                            {
                                cansit = true;
                            }
                        }
                    }

                    if(cansit)
                    {
                        playerIsSat[playerPressed] = true;
                        lastPlayerPressed = playerPressed;
                        await Task.Delay(200);
                        CCSimpleAudioEngine.SharedEngine.PlayEffect(soundSit);


                    }
                    else
                    {
                        buttonIsBlocked[playerPressed] = true;
                        playerIsRetired[playerPressed] = true;
                        lastPlayerPressed = playerPressed;

                        await Task.Delay(200);
                        CCSimpleAudioEngine.SharedEngine.PlayEffect(soundFall);

                    }

                }

            }
        }


        void endRownd()
        {
            rownd++;
            int remaindPlayers = 0;
            int winner = -1;

            for(int i = 0; i < playerIsRetired.Count;i++)
            {
                if(buttonIsBlocked[i]||(!buttonIsBlocked[i]&&!playerIsSat[i]))
                {
                    playerIsRetired[i] = true;
                    if(GameData.orden[i] == 0)
                    {
                        GameData.orden[i] = GameData.players - perdedores;
                        perdedores++;
                    }

                }
                if(!playerIsRetired[i])
                {
                    remaindPlayers++;
                    winner = i;
                    playerIsSat[i] = false;
                    buttonIsBlocked[i] = false;
                }
            }
            timePassed = 0;
            inmusic = false;
            if (remaindPlayers == 1)
            {
                GameData.orden[winner] = 1;
                setEnd(winner);

            }
            else if(remaindPlayers==0)
            {
                winner = lastPlayerPressed;
                setEnd(winner);
            }
            else
            {

                inSplash = true;
                inmusic = false;
                prepareRown();
            }
        }

       async  void setEnd(int winner)
        {
            CCSimpleAudioEngine.SharedEngine.ResumeAllEffects();
            CCSimpleAudioEngine.SharedEngine.StopEffect(musicId);

            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/endwistle");
            AddChild(label);
            label.Text = "¿y el ganador es! ... ";
            await Task.Delay(600);
            if(winner==-1)
            {
                label.Text = " ¡Nadie!. La partida terminó en un empate! ¡malos! \n Los resultados seran distribuidos aleatoriamente. ";
            }
            else
            {
                winner++;
                label.Text = " el jugador! " + winner.ToString() + " ¡felicidades! \n toque para continuar ";
            }
            
            end = true;

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

            if(end&&keyEvent.Keys== CCKeys.Enter)
            {
                returnToMenu();
            }
            if(!started&&keyEvent.Keys== CCKeys.Enter)
            {
                setStart();


            }
            else if(started&& keyEvent.Keys== CCKeys.Z)
            {
                processPress(0);

            }
            else if(started&&keyEvent.Keys== CCKeys.M&&GameData.players>=2)
            {
                processPress(1);
            }
            else if(started&&keyEvent.Keys== CCKeys.Q&&GameData.players>=3)
            {
                processPress(2);
            }
            else if(started&&keyEvent.Keys== CCKeys.P&&GameData.players>=4)
            {
                processPress(3);

            }

        }


        public void returnToMenu()
        {
            
            CCSimpleAudioEngine.SharedEngine.PlayEffect(soundChooce);
            var newScene = new CCScene(Window);
            var silla = new Tablero();
            newScene.AddChild(silla);
            Window.DefaultDirector.ReplaceScene(newScene);

        }

        private CCPoint GetPosicionJugador(int idJugador)
        {
            var bounds = VisibleBoundsWorldspace;
            CCPoint retorno;
            float offset = 50;
            float x = 0;
            float y = 0;
            switch (idJugador)
            {
                case 1:
                    x = bounds.MinX + offset; y = bounds.MinY + offset; break;
                case 2:
                    x = bounds.MaxX - offset; y = bounds.MinY + offset; break;
                case 3:
                    x = bounds.MinX + offset; y = bounds.MaxY - offset; break;
                case 4:
                    x = bounds.MaxX - offset; y = bounds.MaxY - offset; break;
                default:
                    break;

            }
            retorno = new CCPoint(x, y);
            return retorno;
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here

            foreach(CCTouch touch in touches)
                {
                    if (end&& GameData.CheckIfLabelTouched(touch, label))
                    {
                        returnToMenu();
                    }


                        if (!started && GameData.CheckIfLabelTouched(touch,label))
                    {
                        setStart();


                    }
                    else if (started && GameData.CheckIfSpriteTouched(touch,p1button))
                    {
                        processPress(0);

                    }
                    else if (started &&GameData.CheckIfSpriteTouched(touch, p2button) && GameData.players >= 2)
                    {
                        processPress(1);
                    }
                    else if (started &&GameData.CheckIfSpriteTouched(touch, p3button)  && GameData.players >= 3)
                    {
                        processPress(2);
                    }
                    else if (started &&  GameData.CheckIfSpriteTouched(touch, p4button) && GameData.players >= 4)
                    {
                        processPress(3);

                    }

                }
            }
        }
    }
}

