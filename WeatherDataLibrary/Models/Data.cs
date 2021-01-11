using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataLibrary.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string SensorName { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }
}
