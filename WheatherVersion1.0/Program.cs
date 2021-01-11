using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherDataLibrary.DataAccess;
using WeatherDataLibrary.Models;

namespace WeatherVersion1._0
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "TemperaturData.csv");
            string[] data = File.ReadAllLines(path);

            List<Data> weatherData = GetAllData(data);
            List<Sensor> sensors = GetSensor(data);
            foreach (var item in weatherData)
            {
                foreach (var sensor in sensors)
                {
                    if (item.SensorName == sensor.SensorName)
                    {
                        sensor.WeatherData.Add(item);
                    }

                }
            }

            LoadToDatabase(sensors);
            Console.ReadLine();
        }

        private static void LoadToDatabase(List<Sensor> sensors)
        {
            using(var db = new DataContext())
            {
                Console.WriteLine("Lägger till databas");

                db.AddRange(sensors);
                Console.WriteLine("Klar!");
                db.SaveChanges();
            }
        }

        private static List<Sensor> GetSensor(string[] data)
        {
            List<Sensor> sensors = new List<Sensor>();
            List<string> sensorList = new List<string>();

            foreach (var row in data)
            {
                string[] split = row.Split(' ', ',', ',', ',');
                sensorList.Add(split[2]);
            }

            sensorList = sensorList
                .Distinct()
                .ToList();
            foreach (var item in sensorList)
            {
                var sensor = new Sensor();
                sensor.SensorName = item;
                sensors.Add(sensor);
            }

            return sensors;
        }

        public static List<Data> GetAllData(string[] data)
        {
            List<Data> weatherList = new List<Data>();
            foreach (var row in data)
            {
                var weatherData = new Data();

                weatherData.Date = GetDate(row);
                weatherData.Time = GetTime(row);
                weatherData.SensorName = GetSensorName(row);
                weatherData.Temp = GetTemp(row);
                weatherData.Humidity = GetHumidity(row);
                weatherList.Add(weatherData);
            }

            return weatherList;
        }

        private static int GetHumidity(string row)
        {
            int index = 0;

            string[] split = row.Split(' ', ',', ',', ',');

            index = int.Parse(split[4]);

            return index;
        }

        private static double GetTemp(string row)
        {
            double index = 0;

            string[] split = row.Split(' ', ',', ',', ',');

            index = double.Parse(split[3], System.Globalization.CultureInfo.InvariantCulture);

            return index;

        }

        private static string GetSensorName(string row)
        {
            string index = "";

            string[] split = row.Split(' ', ',', ',', ',');

            index = split[2];

            return index;
        }

        private static string GetTime(string row)
        {
            string index = "";

            string[] split = row.Split(' ', ',', ',', ',');

            index = split[1];

            return index;
        }

        private static DateTime GetDate(string row)
        {
            var index = new DateTime();

            string[] split = row.Split(' ', ',', ',', ',');

            index = DateTime.Parse(split[0]);

            return index;
        }
    }
}
