using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataLibrary.Models
{
    public class Sensor
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public List<Data> WeatherData { get; set; } = new List<Data>();
    }
}
