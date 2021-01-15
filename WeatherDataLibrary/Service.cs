using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WeatherDataLibrary.DataAccess;
using WeatherDataLibrary.Models;

namespace WeatherDataLibrary
{
    public class Service
    {
        public static List<Data> DataBase()
        {
            using (var db = new DataContext())
            {
                List<Data> datas = new List<Data>();
                datas = db.Datas.ToList();
                return datas;
            }
        }
        public static List<Day> WarmColdSort(List<Data> datas, string sensorName)
        {
            List<Day> result = new List<Day>();

            var list = datas
                .Where(l => l.SensorName == sensorName)
                .GroupBy(l => l.Date)
                .Select(l => new
                {
                    average = l.Average(l => l.Temp),
                    date = l.Key
                });
            foreach (var item in list)
            {
                Day d = new Day();
                d.Date = item.date;
                d.Temp = item.average;
                result.Add(d);
            }
            return result;
        }

        public static string AverageTemp(List<Data> datas, DateTime dateChoice, string sensorName)
        {
            var temp = datas
                .Where(t => t.SensorName == sensorName && t.Date == dateChoice)
                .Average(t => t.Temp);
            string result = $"{dateChoice.ToShortDateString()}  {Math.Round(temp, 2)}";
            return result;
        }

        public static int NumberOfDays(List<Data> datas)
        {
            int numberOfDays = 0;
            var days = datas
                .GroupBy(d => d.Date);
            foreach (var item in days)
            {
                numberOfDays++;
            }
            return numberOfDays;
        }

        public static string InOrOut()
        {
            int acceptedAnswer = 0;
            string sensorName = "default";
            Console.Write("\n\t[1] Inne" +
                "\n\t[2] Ute" +
                "\n\tVälj: ");
            do
            {
                ConsoleKey insideOutside = Console.ReadKey().Key;
                Console.Clear();
                switch (insideOutside)
                {
                    case ConsoleKey.D1:
                        sensorName = "Inne";
                        acceptedAnswer = 1;
                        break;
                    case ConsoleKey.D2:
                        sensorName = "Ute";
                        acceptedAnswer = 1;
                        break;
                    default:
                        Console.WriteLine("Ogiltig inmatning!");
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                }
            } while (acceptedAnswer < 1);
            return sensorName;
        }

        public static List<Day> WetDrySort(List<Data> datas, string sensorName)
        {
            List<Day> result = new List<Day>();

            var list = datas
                .Where(l => l.SensorName == sensorName)
                .GroupBy(l => l.Date)
                .Select(l => new
                {
                    average = l.Average(l => l.Humidity),
                    date = l.Key
                });
            foreach (var item in list)
            {
                Day d = new Day();
                d.Date = item.date;
                d.Humidity = (int)item.average;
                result.Add(d);
            }
            return result;
        }

        public static List<Day> MoldRiskSort(List<Data> datas, string sensorName)
        {
            List<Day> result = new List<Day>();

            var list = datas
                .Where(l => l.SensorName == sensorName)
                .GroupBy(l => l.Date)
                .Select(l => new
                {
                    humidityAverage = l.Average(l => l.Humidity),
                    //temp = l.Average(l => l.Temp),
                    //humidity = l.Average(l => l.Humidity),
                    tempAverage = l.Average(l => l.Temp),
                    date = l.Key
                    //((fuktighet - 78) * (temperatur / 15)) / 0,22
                });
            var moldList = list
                .Where(m => m.tempAverage >= 0 && m.humidityAverage >= 78)
                .Select(m => new
                {
                    date = m.date,
                    //temp = m.temp,
                    //humidity = m.humidity,
                    moldCalc = ((m.humidityAverage - 78) * (m.tempAverage / 15)) / 0.22
                });
            foreach (var item in moldList)
            {
                //if (item.humidity < 78 || item.temp < 0)
                //{

                //}
                //else
                //{
                //}
                    Day d = new Day();
                    d.Date = item.date;
                    //d.Temp = item.temp;
                    //d.Humidity = (int)item.humidity;
                    d.MoldRisk = item.moldCalc;
                    result.Add(d);
            }
            return result;
        }

    }
}
