using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;
using System.Threading.Tasks;
using System.Threading;


namespace project.cpp.Core
{
    class MaletaLayer : CCLayerColor
    {
        CCSprite fondo;
        CCSprite azafata;
        CCLabel texto;
        CCSprite uno = new CCSprite("images/uno");
        CCSprite dos = new CCSprite("images/dos");
        CCSprite tres = new CCSprite("images/tres");
        Queue<CCSprite> maletas = new Queue<CCSprite>();
        int estado = 0;  //Estado 0 indica que se dan las instrucciones.
        int tiempoUltimotick = 0;

        public MaletaLayer() : base()
        {
            AgregarFondo();
            AgregarPersonajes();
            texto = new CCLabel("Buenos dias pasajeros", "fonts/MarkerFelt", 22, CCLabelFormat.SystemFont);
            texto.Color = CCColor3B.Black;
            AddChild(texto);

        }
        protected override void AddedToScene()
        {
            base.AddedToScene();
            var Bounds = VisibleBoundsWorldspace;
            fondo.Position = Bounds.Center;
            GameData.ResizeBackground(fondo, this);
            GameData.ResizeSprite(azafata, (float)0.6);
            azafata.Position = new CCPoint(100,200);
            texto.Position = Bounds.Center;
            //Agrego listener
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
            CCEventListenerKeyboard keyboardListener = new CCEventListenerKeyboard();
            keyboardListener.OnKeyPressed = onKeyPress;
            AddEventListener(keyboardListener, this);
            Schedule(Update);
        }
        void onKeyPress(CCEventKeyboard keyEvent)
        {
        }

        public void OnTouchesEnded(List<CCTouch> touches, CCEvent e)
        {
            switch (estado)
            {
                case (0):
                    texto.Text = "En este minijuego, debes hacer click a las maletas de tu color cuando pasen.";
                    estado++;
                    break;
                case (1):
                    texto.Text = "El jugador que reuna mas equipaje gana!!";
                    estado++;
                    break;
                case (2):
                    texto.Text = "";
                    estado++;
                    Schedule(MostrarContador);
                    break;
            }
        }

        private void MostrarContador(float dt)
        {
            var center = VisibleBoundsWorldspace.Center;
            tiempoUltimotick++;
            if(tiempoUltimotick * dt > 0.4)
            {
                tiempoUltimotick = 0;
                if (estado == 3)
                {
                    AddChild(tres);
                    tres.Position = center;
                    estado++;
                }
                else if (estado == 4)
                {
                    RemoveChild(tres);
                    AddChild(dos);
                    dos.Position = center;
                    estado++;
                }
                else if (estado == 5)
                {
                    RemoveChild(dos);
                    AddChild(uno);
                    uno.Position = center;
                    estado++;
                }
                else if (estado == 6)
                {
                    estado++;
                    RemoveChild(uno);
                    RemoveChild(azafata);
                    Schedule(FlujoDelJuego);
                    Unschedule(MostrarContador);
                }
            }
  
        }

        private void FlujoDelJuego(float dt) //Comienza con estado = 7. Indica que se empiezan recien a crear maletas
        {
            switch (estado)
            {
                case (7):
                    maletas.Enqueue(new CCSprite("images/d1"));
                    CCSprite maleta = maletas.Peek();
                    GameData.ResizeSprite(maleta, (float)0.5);
                    AddChild(maleta);
                    maleta.Position = new CCPoint(VisibleBoundsWorldspace.MaxX - 150, VisibleBoundsWorldspace.Center.Y -50);
                    estado++;
                    tiempoUltimotick++;
                    break;
                case (8):
                    foreach (CCSprite element in maletas)
                    {
                        element.PositionX = element.PositionX - 7;
                    }
                    if(tiempoUltimotick * dt > 0.5)
                    {
                        CCSprite maletiwi = new CCSprite("images/d1");
                        GameData.ResizeSprite(maletiwi, (float)0.5);
                        AddChild(maletiwi);
                        maletiwi.Position = new CCPoint(VisibleBoundsWorldspace.MaxX - 150, VisibleBoundsWorldspace.Center.Y - 50);
                        maletas.Enqueue(maletiwi);
                        tiempoUltimotick = 0;
                    }
                    tiempoUltimotick++;
                        break;
                    
            }


        }
        private void ReturnToMenu()
        {
            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/chooce");
            var newScene = new CCScene(Window);
            var menu = new IntroLayer();
            newScene.AddChild(menu);
            Window.DefaultDirector.ReplaceScene(newScene);
        }
        private void DerrotaJugador(int idJugador)  //recibe entero entre 1 y 4.
        {
        }

        private CCColor3B GetColorJugador(int idJugador)
        {
            CCColor3B color;
            switch (idJugador)
            {
                case 1:
                    color = CCColor3B.Red;
                    break;
                case 2:
                    color = CCColor3B.Blue; break;
                case 3:
                    color = CCColor3B.Yellow; break;
                default:
                    color = CCColor3B.Green; break;
            }
            return color;
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

        private void AgregarPersonajes()
        {
            azafata = new CCSprite("images/azafata");
            AddChild(azafata);
        }
        private void AgregarFondo()
        {
            fondo = new CCSprite("images/maletas_pc");
            AddChild(fondo);
        }
    }
}
