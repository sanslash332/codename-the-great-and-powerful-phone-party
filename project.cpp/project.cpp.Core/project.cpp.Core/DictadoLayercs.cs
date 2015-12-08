using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using CocosDenshion;
using System.Threading.Tasks;


namespace project.cpp.Core
{
    class DictadoLayercs : CCLayer
    {
        CCSprite fondo;
        CCSprite[] botones;
        CCLabel debug;
        bool[] jugadoresActivos;
        bool win = false;
        bool contesto = true; //Es false si es que un jugador no responde a su llamado. Cuando esto pase debería perder.
        double tiempoDeRespuesta = 1.5; //Mientras más bajo, los jugadores tendrán menos tiempo para presionar su botón.
        int tiempoUltimoSonido = 0;
        Random random = new Random();
        int llamando = 0; //Almacena al jugador al que se le pide presionar el botón. 0 Indica a ninguno, y el resto a su jugador respectivo.
        public DictadoLayercs() : base()
        {
            AgregarFondo();
            debug = new CCLabel("", "fonts/MarkerFelt", 22, CCLabelFormat.SystemFont);
            AddChild(debug);
            //Inicializa a los jugadores que comienzan el juego y crea sus botones respectivos.
            AgregarBotones();
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var Bounds = VisibleBoundsWorldspace;
            debug.Color = CCColor3B.Black;
            debug.Position = Bounds.Center;
            fondo.Position = Bounds.Center;
            for(int i=0; i<botones.Length; i++)
            {
                botones[i].Position = GetPosicionJugador(i+1);
            }

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
            if (!win)
            {
                int tecla = 5;
                if (keyEvent.Keys == CCKeys.D1)
                {
                    tecla = 0;
                }
                else if (keyEvent.Keys == CCKeys.D2)
                {
                    tecla = 1;
                }
                else if (keyEvent.Keys == CCKeys.D3)
                {
                    tecla = 2;
                }
                else if (keyEvent.Keys == CCKeys.D4)
                {
                    tecla = 3;
                }
                else
                {
                    return;
                }
                if (llamando == tecla + 1  && jugadoresActivos[tecla])
                {
                    debug.Text = "Correcto!";
                    debug.Color = GetColorJugador(tecla + 1);

                    contesto = true; //Indicarle al programa que el jugador no perdió(?)
                    CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin");
                }
                else
                {
                    if (tecla >= 0 && tecla <= 3)
                    {
                        DerrotaJugador(tecla + 1);
                    }
                }
            }

            else
            {
                ReturnToMenu();
            }
        }
        public override void Update(float dt)
        {
            if (!win)
            {
 

                tiempoUltimoSonido++;
                if (tiempoUltimoSonido * dt > tiempoDeRespuesta)
                {
                    debug.Text = "";
                    if (contesto == false)  //Jugador se demoró mucho en contestar
                    {
                        DerrotaJugador(llamando);
                    }
                    string sonido = GetSonidoRandom();
                    CCSimpleAudioEngine.SharedEngine.PlayEffect(sonido);
                    tiempoUltimoSonido = 0;
                    tiempoDeRespuesta -= 0.02;
                }
            }
        }
        public void OnTouchesEnded(List<CCTouch> touches, CCEvent e)
        {
            if (!win)
            {
                foreach (CCTouch touch in touches)
                {
                    for (int i = 0; i < botones.Length; i++)
                    {
                        if (GameData.CheckIfSpriteTouched(touch, botones[i]))
                        {
                            if (llamando == i + 1  && jugadoresActivos[i])
                            {
                                debug.Text = "Correcto!";
                                debug.Color = GetColorJugador(i + 1);
                                contesto = true;
                                CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/coin");
                            }
                            else
                            {
                                DerrotaJugador(i + 1);
                            }
                        }
                    }
                }
            }
            else {
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
        private void DerrotaJugador(int idJugador)  //recibe entero entre 1 y 4.
        {
            jugadoresActivos[idJugador-1] = false;
            botones[idJugador-1].Color = CCColor3B.Black;
            debug.Text = "Mal! :(";
         //   CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/fall");
            if (checkVictoria() != -1)
            {
                debug.Color = GetColorJugador(checkVictoria());
                debug.Text = "El ganador es el jugador " + checkVictoria() + ".Presiona cualquier tecla";
            }
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
            float x=0;
            float y=0;
            switch (idJugador){
                case 1:
                    x = bounds.MinX + offset; y = bounds.MinY + offset; break;
                case 2:
                    x = bounds.MaxX - offset ; y = bounds.MinY + offset; break;
                case 3:
                    x = bounds.MinX + offset; y = bounds.MaxY - offset;  break;
                case 4:
                    x = bounds.MaxX - offset; y = bounds.MaxY - offset; break;
                default:
                    break;

            }
            retorno = new CCPoint(x, y);
            return retorno;
        }

        private void AgregarFondo()
        {
            fondo = new CCSprite("images/silla_pc");
            AddChild(fondo);
        }

        private void AgregarBotones()
        {
            jugadoresActivos = new bool[4];
            botones = new CCSprite[GameData.players];
            for (int i = 0; i < GameData.players; i++)
            {
                jugadoresActivos[i] = true;
                botones[i] = new CCSprite("images/p"+ (i+1) + "_logo");
                AddChild(botones[i]);
            }
        }

        private string GetSonidoCorrecto()  //Retorna el string que lleva a un sónido con el número de algún jugador activo, y modifica la variable llamando.
        {
            int jugador;
            contesto = false; //Indicar que se solicita que algún jugador conteste.
            while (true)
            {
                jugador = random.Next(1, GameData.players+1);
                if (jugadoresActivos[jugador - 1])  //Verifica si el jugador sigue jugando
                {
                    llamando = jugador;
                    if (jugador != 3)
                    {
                        return "sounds/Numeros/" + jugador + "_" + random.Next(1, 3);
                    }
                    else
                    {
                        return "sounds/Numeros/3_1";
                    }
                }
            }
        }
        private string GetSonidoIncorrecto()//Lo mismo que arriba pero con números que no le sirven a nadie.
        {
            string retorno;
            int sonido = random.Next(0, 11); //Random entre 0 y 10 (inclusive). Por el momento hay 11 sónidos "inútiles"
            llamando = 0; //Para saber que todos pierden si apretan el boton.
            switch (sonido)
            {
                case 0:
                    retorno = "sounds/Numeros/12"; break;
                case 1:
                    retorno = "sounds/Numeros/13"; break;
                case 2:
                    retorno = "sounds/Numeros/37"; break;
                case 3:
                    retorno = "sounds/Numeros/40"; break;
                case 4:
                    retorno = "sounds/Numeros/49"; break;
                case 5:
                    retorno = "sounds/Numeros/78"; break;
                case 6:
                    retorno = "sounds/Numeros/200"; break;
                case 7:
                    retorno = "sounds/Numeros/341"; break;
                case 8:
                    retorno = "sounds/Numeros/471"; break;
                case 9:
                    retorno = "sounds/Numeros/78"; break;  //Elimine el 3208 porque se demoraba mucho en decirlo
                default:
                    retorno = "sounds/Numeros/4000"; break;
            }
            return retorno;
        }

        private string GetSonidoRandom() //retorna algún sonido cualquiera
        {
            if(random.Next(0, 100) < 75)
            {
                return GetSonidoCorrecto();
            }
            return GetSonidoIncorrecto();
        }

        private int checkVictoria()
        {
            int restantes = 0;
            int winner = 0;
            for(int i=0; i<jugadoresActivos.Length; i++)
            {
                if(jugadoresActivos[i] == true)
                {
                    restantes++;
                    winner = i;
                }
            }
            if(restantes > 1)
            {
                return -1;
            }
            win = true;  //Para que dejen de sonar sonidos random.
            return winner+1;
        }
    }
}
