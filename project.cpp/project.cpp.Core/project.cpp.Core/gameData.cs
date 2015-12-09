using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace project.cpp.Core
{
    public static class GameData
    {
        //una clase simplemente para guardar puntos, turnos, jugadores y cualquier cosa importante que haya que patear de pantalla en pantalla
        public static int players=1;
        public static int[] pos = new int[4]; //Almacenan la casilla en que se encuentran los jugadores en el tablero.
        public static int[] scores;
        public static double x;
        public static double y;
        public static bool primeraVez = true;
        public static int currentTurn=0;
        public static int[] orden = new int[4]; //Orden de los jugadores despues de un minijuego.
		
		public static int[] levels;
		private static double[] boxX_pc = {41.8, 49.5, 55.8, 62.4, 71.5, 75.0, 74.1, 67.0, 57.9, 48.6, 40.0, 39.8, 47.0, 55.4, 64.0, 72.0, 77.1, 78.0, 71.6, 65.9, 59.6, 51.8, 43.8, 41.0, 44.5, 50.1, 58.5, 64.6, 71.1, 78.8, 75.0, 67.8, 60.6, 56.9, 47.2}; //En porcentaje respecto al sprite de fondo
		private static double[] boxY_pc = {87.5, 90.4, 84.5, 90.4, 91.2, 79.2, 68.9, 74.0, 72.3, 74.7, 74.2, 62.4, 63.7, 63.3, 63.7, 59.2, 52.2, 42.0, 38.4, 46.6, 50.9, 53.6, 53.4, 43.1, 33.0, 41.6, 38.2, 32.2, 27.9, 27.5, 17.4, 14.0, 16.9, 27.2, 19.3};
		private static double[] boxX_android = {13.1,30.3,43.8,58.4,76.8,85.8,83.1,67.8,48.7,28.5,10.5,9.7,25.5,43.1,61.4,79.0,89.5,91.0,77.9,65.5,52.4,35.6,18.7,13.1,19.9,32.2,50.2,63.2,77.1,93.2,84.5,69.0,53.6,46.4,26.0}; //En porcentaje respecto al sprite de fondo
		private static double[] boxY_android = {71.4,73.5,69.7,73.8,74.6,65.8,58.5,62.1,61.1,62.5,62.4,53.9,54.8,54.6,55.1,51.7,46.7,38.9,36.8,42.7,45.8,47.7,47.8,40.4,33.1,39.3,36.8,32.8,29.6,28.9,22.0,19.5,21.8,28.8,23.4};
		
		private static Random r = new Random();		
		
		
		public static void ArreglarCosas() //Da vuelta el problema con las coordendas Y que estaban al revez. Además inicializa algunos arreglos.
        {
            if (primeraVez)
            {
                for (int i = 0; i < boxY_pc.Count(); i++)
                {
                    boxY_pc[i] = 100 - boxY_pc[i];
                }

                for (int i = 0; i < 4; i++)
                {
                    if (pos[i] == 0)
                        pos[i] = 1;
                }
                primeraVez = false;
            }

        }
        public static bool CheckIfLabelTouched(CCTouch touch, CCLabel label)
        {
            CCRect BoundingBox = label.BoundingBox;

            //Tuve que agregar un offset de 20 en al minY y maxY porque la boundingBox de las labels estaba mala.
            if (touch.Location.X > BoundingBox.MinX && touch.Location.X < BoundingBox.MaxX && touch.Location.Y < BoundingBox.MaxY + 20 && touch.Location.Y > BoundingBox.MinY)
            {
                return true;
            }
            return false;
        }

        public static CCPoint GetPosicionJugador(int idJugador, CCLayerColor layer)
        {
            var bounds =layer.VisibleBoundsWorldspace;
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
        public static bool CheckIfSpriteTouched(CCTouch touch, CCSprite sprite)
        {
            CCRect BoundingBox = sprite.BoundingBox;

            //Tuve que agregar un offset de 20 en al minY y maxY porque la boundingBox de las labels estaba mala.
            if (touch.Location.X > BoundingBox.MinX && touch.Location.X < BoundingBox.MaxX && touch.Location.Y < BoundingBox.MaxY + 20 && touch.Location.Y > BoundingBox.MinY)
            {
                return true;
            }
            return false;
        }

        public static CCColor3B GetColorJugador(int idJugador)
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

        public static void ResizeBackground(CCSprite background, CCLayerColor layer) //Escala el fondo para que sea del mismo tamaño que la pantalla.
        {
             var bounds = layer.VisibleBoundsWorldspace;
            float xlength = bounds.Size.Width;
            float ylength = bounds.Size.Height;
            x = xlength;
            y = ylength;
             background.ScaleX = xlength / background.ContentSize.Width;
             background.ScaleY = ylength / background.ContentSize.Height;
        }

        public static void ResizeSprite(CCSprite sprite, float factor)
        {
            sprite.ScaleX = factor;
            sprite.ScaleY = factor;

        }
		public static double[] getCoords(int level, bool isPc){			
			double[] output;
			double random1 = r.Next(0, 10);
            random1 = random1 / 10;
            double random2 = r.Next(0, 10);
            random2 = random2 / 10;
            if (isPc) output= new double[]{boxX_pc[level-1]-1+random1 , boxY_pc[level-1]-1+random2};
            else output = new double[] { boxX_android[level - 1] - 1 + random1, boxY_android[level - 1] - 1 + random2 };
			
			return output;
		}
		
        public static CCPoint getPointMapa(int posicion, CCLayerColor layer)
        {
            var bounds = layer.VisibleBoundsWorldspace;
            float xlength = bounds.Size.Width;
            float ylength = bounds.Size.Height;
            double[] puntos = getCoords(posicion, true);
            CCPoint retorno = new CCPoint((float)puntos[0]*xlength/100, (float)puntos[1]*ylength/100);
            return retorno;
        }

        public static int getLugar(int lugar)  //Retorna el id del jugador que quedó en la posición lugar en el ultimo minijuego.
        {
            for(int i=0; i<orden.Length; i++)
            {
                if(orden[i] == lugar)
                {
                    return i+1;
                }
            }
            return 0;
        }
    }
}
