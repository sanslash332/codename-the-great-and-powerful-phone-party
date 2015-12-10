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
        List<CCSprite> iconos = new List<CCSprite>();
        List<CCLabel> puntajes = new List<CCLabel>();
        List<int> puntos = new List<int>();
        List<int> contadorMaletas = new List<int>(); //Almacena la cantidad de maletas que ha salido de cada color.
        float tiempoSiguienteMaleta = 0.5f;
        float tiempoRestante = 2500f; //"Ticks" que dura la ronda. Reemplazar por 3000.

        Queue<CCSprite> maletas = new Queue<CCSprite>();
        Random random = new Random();
        int estado = 0;  //Estado 0 indica que se dan las instrucciones.
        int tiempoUltimotick = 0;
        int mid;

        public MaletaLayer() : base()
        {
            CCSimpleAudioEngine.SharedEngine.PreloadEffect("bgm/maletas");
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

            mid = CCSimpleAudioEngine.SharedEngine.PlayEffect("bgm/maletas2", true);


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
                    texto.Text = "Que no se te pase ningun equipaje de tu color!\nEl que obtenga un porcentaje mas alto de recogidas gana.";
                    estado++;
                    break;
                case (2):
                    texto.Text = "";
                    estado++;
                    Schedule(MostrarContador);
                    break;
                 case(8):   //Cuando ya van saliendo maletas
                    foreach (CCTouch touch in touches)
                    {
                        foreach (CCSprite maleta in maletas)
                        {
                            if (GameData.CheckIfSpriteTouched(touch, maleta))
                            {
                                int idJugador = (int)maleta.UserData;
                                puntos[idJugador - 1]++;   //En UserData almaceno un entero con el id del jugador, desde el 1 al 4. Le resto uno para que coincida con las posiciones del arreglo.
                                ActualizarPuntaje(idJugador);
                                RemoveChild(maleta);
                            }
                        }
                    }
                    break;
                case (10)://Ya hay ganador.
                    texto.Text = "Felicitaciones al jugador " + VerificarGanador();
                    estado++;
                    break;
                case (11):
                    texto.Text = "El jugador " + VerificarPerdedor() + " va a tener que esforzarse mas. \nULTIMO LUGAR!!!";
                    estado++;
                    break;
                case (12):
                    texto.Text = "Preparense para volver al tablero!.";
                    estado++;
                    break;
                case (13):
                    ReturnToMenu();
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
                    AgregarPuntajes();
                    GenerarMaletaRandom();
                    estado++;
                    tiempoUltimotick++;
                    break;
                case (8):
                    if(maletas.Count > 0)
                    {
                        foreach (CCSprite element in maletas)
                        {
                            element.PositionX = element.PositionX - 10;

                             for(int i=1; i< GameData.players+1; i++)
                                {
                                    ActualizarPuntaje(i);
                                }
                        }

                    }

                    if (tiempoUltimotick * dt > tiempoSiguienteMaleta)
                    {
                        GenerarMaletaRandom();
                        tiempoSiguienteMaleta = ((float)random.NextDouble()/2) + 0.15f;
                        tiempoUltimotick = 0;
                    }
                    tiempoUltimotick++;
                    tiempoRestante--;
                    if(tiempoRestante <= 0)
                    {
                        estado++;
                    }
                        break;
                case (9):
                    AddChild(azafata);
                    texto.Text = "Se acabó el tiempo!";
                    AddChild(texto);
                    estado++;
                    break;
            }



        }
        private void ReturnToMenu()
        {
            CCSimpleAudioEngine.SharedEngine.StopEffect(mid);
            ActualizarOrdenGameData();
            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/chooce");
            var newScene = new CCScene(Window);
            var menu = new Tablero();
            newScene.AddChild(menu);
            Window.DefaultDirector.ReplaceScene(newScene);
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

        private void GenerarMaletaRandom()
        {
            GenerarMaleta(random.Next(1, GameData.players+1));
        }
        private void GenerarMaleta(int idJugador)  //idJugador es un entero entre 1 y 4.
        {
            contadorMaletas[idJugador - 1]++;
            CCSprite maletiwi = new CCSprite("images/d" + idJugador);
            GameData.ResizeSprite(maletiwi, (float)0.5);
            AddChild(maletiwi);
            maletiwi.Position = new CCPoint(VisibleBoundsWorldspace.MaxX - 150, VisibleBoundsWorldspace.Center.Y - 50);
            maletiwi.UserData = idJugador;
            maletas.Enqueue(maletiwi);
        }

        private void AgregarPuntajes()
        {
            for(int i=1; i< GameData.players + 1; i++)
            {
                CCSprite sprite = new CCSprite("images/p" + i + "_logo");
                CCLabel label = new CCLabel("100%", "fonts/MarkerFelt", 22, CCLabelFormat.SystemFont);
                label.Color = CCColor3B.Black;
                iconos.Add(sprite);
                puntajes.Add(label);
                puntos.Add(0);
                contadorMaletas.Add(0);
                CCPoint posicionSprite = new CCPoint(50 + (i * 200), 650);
                CCPoint posicionLabel = new CCPoint(80 + (i * 200), 650);
                GameData.ResizeSprite(sprite, 0.6f);
                sprite.Position = posicionSprite;
                label.Position = posicionLabel;
                AddChild(sprite);
                AddChild(label);
            }
        }

        private void ActualizarPuntaje(int idJugador)
        {
            //  puntajes[idJugador-1].Text = (puntos[idJugador-1] / contadorMaletas[idJugador-1]) * 100 + "%";
            if(idJugador > 0)
            {
                if (contadorMaletas[idJugador - 1] != 0)
                    puntajes[idJugador - 1].Text = (puntos[idJugador - 1] * 100 / contadorMaletas[idJugador - 1]) + "%";

            }
        }

        private int VerificarGanador()  //Retorna id del jugador que gano (mayor porcentaje).
        {
            int winner = 1;
            double puntajeWinner = 0;
            for(int i=0; i< puntos.Count; i++)
            {
                if (((puntos[i] * 100) / (contadorMaletas[i]+1)) > puntajeWinner)
                {
                    winner = i + 1;
                    puntajeWinner = ((puntos[i] * 100) / (contadorMaletas[i] + 1));
                }       
            }
            return winner;
        }

        private int VerificarPerdedor()  //Retorna id del jugador que perdio (menor porcentaje).
        {
            int lusi = 1;
            double puntajelusi = 100;
            for (int i = 0; i < puntos.Count; i++)
            {
                if ((puntos[i] * 100 / (contadorMaletas[i]+1)) < puntajelusi)
                {
                    lusi = i + 1;
                    puntajelusi = (puntos[i] * 100 / (contadorMaletas[i]+1));
                }
            }
            return lusi;
        }

        private void ActualizarOrdenGameData()  //Modifica el arreglo del gamedata con el orden de los jugadores según los porcentajes.
        {
            for(int i=0; i < GameData.players; i++)
            {
                GameData.orden[i] = GetOrden(i + 1);
            }
        }

        private int GetOrden(int jugadorId) //retorna el lugar que consiguio en el juego el jugador con id jugadorId.
        {
            int retorno = GameData.players;
            for(int i=0; i<GameData.players; i++)
            {
                if( ((puntos[i]*100)/(contadorMaletas[i]+1)) < (puntos[jugadorId-1] * 100) / (contadorMaletas[jugadorId-1] + 1))
                {
                    retorno--;
                }
            }
            return retorno;
        }

    }

}
