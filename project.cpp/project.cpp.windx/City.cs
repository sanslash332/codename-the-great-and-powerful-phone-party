using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project.cpp.windx
{
    class City
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public String name { get; set; }

        public City(String name, double lat, double lon)
        {
            this.name = name;
            this.lat = lat;
            this.lon = lon;
        }


    }
}
