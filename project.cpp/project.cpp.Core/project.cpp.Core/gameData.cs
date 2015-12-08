﻿using System;
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
        public static int[] scores;
        public static double x;
        public static double y;
        
        public static int currentTurn=0;
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


        public static bool CheckIfSpriteTouched(CCTouch touch, CCSprite label)
        {
            CCRect BoundingBox = label.BoundingBox;

            //Tuve que agregar un offset de 20 en al minY y maxY porque la boundingBox de las labels estaba mala.
            if (touch.Location.X > BoundingBox.MinX && touch.Location.X < BoundingBox.MaxX && touch.Location.Y < BoundingBox.MaxY + 20 && touch.Location.Y > BoundingBox.MinY)
            {
                return true;
            }
            return false;
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
    }
}
