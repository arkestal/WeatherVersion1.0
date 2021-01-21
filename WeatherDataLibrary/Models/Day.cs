using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataLibrary.Models
{
    public class Day
    {
        public DateTime Date { get; set; }
        public double Temp { get; set; }
        public int Humidity { get; set; }
        public double MoldRisk { get; set; }
    }
}
