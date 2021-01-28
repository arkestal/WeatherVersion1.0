using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using WeatherDataLibrary.Models;

namespace WeatherDataLibrary.DataAccess
{
    public class DbService
    {
        public static void FixDb()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "TemperaturData.csv");
            string[] data = File.ReadAllLines(path);

            List<Data> weatherData = GetAllData(data);
            List<Sensor> sensors = GetSensor(data);
            foreach (var sensor in sensors)
            {
                sensor.WeatherData = new List<Data>();
            }
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
        }

        public static void LoadToDatabase(List<Sensor> sensors)
        {
            using (var db = new DataContext())
            {
                //Adding database
                db.AddRange(sensors);
                db.SaveChanges();
            }
        }
        public static List<Data> DataBase()
        {
            using (var db = new DataContext())
            {
                List<Data> datas = new List<Data>();
                datas = db.Datas.ToList();
                return datas;
            }
        }
        public static List<Sensor> GetSensor(string[] data)
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
        public static double GetTemp(string row)
        {
            double index = 0;

            string[] split = row.Split(' ', ',', ',', ',');

            index = double.Parse(split[3], System.Globalization.CultureInfo.InvariantCulture);

            return index;

        }
        public static string GetSensorName(string row)
        {
            string index = "";

            string[] split = row.Split(' ', ',', ',', ',');

            index = split[2];

            return index;
        }
        public static string GetTime(string row)
        {
            string index = "";

            string[] split = row.Split(' ', ',', ',', ',');

            index = split[1];

            return index;
        }
        public static DateTime GetDate(string row)
        {
            var index = new DateTime();

            string[] split = row.Split(' ', ',', ',', ',');

            index = DateTime.Parse(split[0]);

            return index;
        }
        public static int GetHumidity(string row)
        {
            int index = 0;

            string[] split = row.Split(' ', ',', ',', ',');

            index = int.Parse(split[4]);

            return index;
        }
    }
}
