using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;
using System.Threading.Tasks;
using System.Threading;


namespace project.cpp.Core
{
    class Tablero : CCLayerColor
    {
        //Aqui van los atributos.
        CCSprite fondo;
        CCSprite piloto;
        CCSprite[] jugadores = new CCSprite[GameData.players];
        CCLabel texto;
        Random random = new Random();
        bool fin = false;
        int estado = 0;
        int multiplicador; //Usado para definir el avance aleatorio despues de jugar un minijuego.
        string soundPass = "sounds/pass";
        string musicMapa = "bgm/bgm2";
        public Tablero() : base()
        {
            if (!GameData.primeraVez)
            {
                estado = 10; //Se salta todas las instrucciones iniciales.
            }
            GameData.ArreglarCosas();

            AgregarFondo();
            AgregarPersonajes();
            AgregarJugadores();
            texto = new CCLabel("Bienvenido a bordo muchachos!.", "fonts/MarkerFelt", 16, CCLabelFormat.SystemFont);
            texto.Color = CCColor3B.Black;
            AddChild(texto);

        }
        protected override void AddedToScene()
        {
            base.AddedToScene();
            CCSimpleAudioEngine.SharedEngine.PlayEffect(musicMapa, true);

            var Bounds = VisibleBoundsWorldspace;
            fondo.Position = Bounds.Center;
            GameData.ResizeBackground(fondo, this);
            GameData.ResizeSprite(piloto, 0.7f);
            piloto.Position = new CCPoint(50, 100);
            texto.Position = new CCPoint(200, 500);
            for(int i=0; i< GameData.players; i++)
            {
                jugadores[i].Position = GameData.getPointMapa(GameData.pos[i], this);
            }
            //Agrego listener
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
            CCEventListenerKeyboard keyboardListener = new CCEventListenerKeyboard();
            keyboardListener.OnKeyPressed = onKeyPress;
            AddEventListener(keyboardListener, this);
        }
        void onKeyPress(CCEventKeyboard keyEvent)
        {
            if(keyEvent.Keys== CCKeys.Enter||keyEvent.Keys== CCKeys.Space)
            {



                CCSimpleAudioEngine.SharedEngine.PlayEffect(soundPass);
            }
        }

        public void OnTouchesEnded(List<CCTouch> touches, CCEvent e)
        {
            CCSimpleAudioEngine.SharedEngine.PlayEffect(soundPass);

            if (!fin)
            {
                switch (estado)
                {
                    case (0):
                        texto.Text = "Desde ahora en adelante cada \nuno de ustedes tendra un color";
                        estado++;
                        break;
                    case (1):
                        texto.Text = "Jugador 1 sera ROJO.";
                        texto.Color = CCColor3B.Red;
                        estado++;
                        break;
                    case (2):
                        texto.Text = "Jugador 2 sera AZUL";
                        texto.Color = CCColor3B.Blue;
                        if (GameData.players > 2)
                        {
                            estado++;
                        }
                        else
                        {
                            estado = 5;
                        }
                        break;
                    case (3):
                        texto.Text = "Jugador 3 sera AMARILLO";
                        texto.Color = CCColor3B.Yellow;
                        if (GameData.players > 3)
                        {
                            estado++;
                        }
                        else
                        {
                            estado = 5;
                        }
                        break;
                    case (4):
                        texto.Text = "Jugador 4 sera VERDE";
                        texto.Color = CCColor3B.Green;
                        estado++;
                        break;
                    case (5):
                        texto.Text = "Se jugarán minijuegos hasta que \nalguno de ustedes alcance la meta.";
                        texto.Color = CCColor3B.Black;
                        estado++;
                        break;
                    case (6):
                        texto.Text = "Luego de cada minijuegos, avanzaran una\ncantidad aleatoria de pasos.";
                        estado++; break;
                    case (7):
                        texto.Text = "Pero tranquilos!. Si ganan tendran\nuna mayor posibilidad de \navanzar mucho más!.";
                        estado++;
                        break;
                    case (8):
                        texto.Text = "Que comience el primer minijuego!";
                        estado++;
                        break;
                    case (9):
                        GoToMiniJuego();
                        break;
                    case (10):
                        texto.Text = "Oh. Así que el ganador fue el jugador " + GameData.getLugar(1);
                        texto.Color = GameData.GetColorJugador(GameData.getLugar(1));
                        estado++;
                        break;
                    case (11):
                        multiplicador = random.Next(1, 4);
                        texto.Text = "El jugador " + GameData.getLugar(1) + " avanzará" + (4 * multiplicador) + " casillas";
                        texto.Color = GameData.GetColorJugador(GameData.getLugar(1));
                        //Avance primer lugar
                        GameData.pos[GameData.getLugar(1) - 1] += multiplicador * 4;
                        VerificarSiGano(GameData.getLugar(1));
                        jugadores[GameData.getLugar(1) - 1].Position = GameData.getPointMapa(GameData.pos[GameData.getLugar(1) - 1], this);
                        //
                        estado++;
                        break;
                    case (12):

                        multiplicador = random.Next(1, 4);
                        texto.Text = "El jugador " + GameData.getLugar(2) + " avanzará " + (3 * multiplicador) + " casillas";
                        texto.Color = GameData.GetColorJugador(GameData.getLugar(2));

                        if (GameData.players > 2)
                        {
                            estado++;
                        }
                        else
                        {
                            estado = 15; //Se salta jugadores 3 y 4.
                        }
                        //Avance segundo lugar
                        GameData.pos[GameData.getLugar(2) - 1] += multiplicador * 3;
                        VerificarSiGano(GameData.getLugar(2));

                        jugadores[GameData.getLugar(2) - 1].Position = GameData.getPointMapa(GameData.pos[GameData.getLugar(2) - 1], this);
                        //
                        break;
                    case (13):

                        multiplicador = random.Next(1, 4);
                        texto.Text = "El jugador " + GameData.getLugar(3) + " avanzará " + (2 * multiplicador) + " casillas";
                        texto.Color = GameData.GetColorJugador(GameData.getLugar(3));

                        if (GameData.players > 3)
                        {
                            estado++;
                        }
                        else
                        {
                            estado = 15; //Se salta al jugador 4.
                        }
                        //Avance tercer lugar
                        GameData.pos[GameData.getLugar(3) - 1] += multiplicador * 2;
                        VerificarSiGano(GameData.getLugar(3));

                        jugadores[GameData.getLugar(3) - 1].Position = GameData.getPointMapa(GameData.pos[GameData.getLugar(3) - 1], this);
                        //
                        break;
                    case (14):
                        multiplicador = random.Next(1, 4);
                        texto.Text = "El jugador " + GameData.getLugar(4) + " avanzará " + (1 * multiplicador) + " casilla(s)";
                        texto.Color = GameData.GetColorJugador(GameData.getLugar(4));
                        estado++;
                        //Avance cuarto lugar
                        GameData.pos[GameData.getLugar(4) - 1] += multiplicador * 1;
                        VerificarSiGano(GameData.getLugar(4));

                        jugadores[GameData.getLugar(4) - 1].Position = GameData.getPointMapa(GameData.pos[GameData.getLugar(4) - 1], this);

                        //
                        break;
                    case (15):
                        texto.Text = "Preparense para el siguiente minijuego!!";
                        texto.Color = CCColor3B.Black;

                        estado++;
                        break;
                    case (16):
                        GoToMiniJuego(); break;
                }
            }
            else
            {
                GameData.pos = new int[4];
                ReturnToMenu();
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

        private void GoToMiniJuego()
        {
            CCSimpleAudioEngine.SharedEngine.StopAllEffects();
            int go_to = random.Next(1, 4);
            CCScene newScene = new CCScene(Window);
            CCLayer nextLayer;
            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/chooce");
            newScene = new CCScene(Window);
            switch (go_to)
            {
                case (1):
                    nextLayer = new SillaMusicalLayer();
                    break;
                case (2):
                    nextLayer = new DictadoLayercs();
                    break;
                default:
                    nextLayer = new MaletaLayer();
                    break;
            }
            newScene.AddChild(nextLayer);
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
            piloto = new CCSprite("images/piloto");
            AddChild(piloto);
        }
        private void AgregarFondo()
        {
            fondo = new CCSprite("images/mapa_pc");
            AddChild(fondo);
        }

        private void AgregarJugadores()
        {
            jugadores = new CCSprite[GameData.players];
            for (int i = 0; i < GameData.players; i++)
            {
                jugadores[i] = new CCSprite("images/p" + (i + 1) + "_logo");
                GameData.ResizeSprite(jugadores[i], 0.2f);
                AddChild(jugadores[i]);
            }
        }

        private void VerificarSiGano(int idJugador)  //retorna true si es que el jugador llego a la meta.
        {
            if(GameData.pos[idJugador-1] >= 35)
            {
                GameData.pos[idJugador - 1] = 35;  //Evita que se pase de 35.
                texto.Text = "El jugador " + idJugador +" ha llegado \na la meta!!!. Buen trabajo!!\nToca la pantalla para volver a jugar.";
                fin = true;
            }
        }
    }

}
