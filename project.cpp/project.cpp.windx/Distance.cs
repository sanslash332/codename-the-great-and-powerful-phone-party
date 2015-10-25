using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace project.cpp.windx
{
    class Distance
    {

        public double getDeg(City from, City to) //retorna ángulo entre ciudades, el norte es 0° clockwise
        {
            double deltaLat = Math.Abs(from.lat - to.lat);
            double deltaLon = Math.Abs(from.lon - to.lon);
            double tan = deltaLon / deltaLat;
            double angle = Math.Atan(tan);            
            return angle;
        }

        public double getDistance(City from, City to) //obtiene distancia plana entre dos ciudades
        {
            double deltaLat = Math.Abs(from.lat - to.lat);
            double deltaLon = Math.Abs(from.lon - to.lon);
            double distance = Math.Sqrt(Math.Pow(deltaLat,2)+ Math.Pow(deltaLon, 2));
            return distance;
        }

        public double getDistance(double angleFromCities, double angleFromUser, double distanceBetweenCities) 
            //Calcula distancia entre recta dada por el usuario y recta real.
        {
            double alpha = Math.Abs(angleFromCities - angleFromUser);
            if (alpha > 180)
            {
                alpha = 360 - alpha;
            }
            double distance = distanceBetweenCities * Math.Sin(alpha);
            return distance;
        }
        
        


    }
}
