using System;
using System.Collections.Generic;
using System.Linq;
using WeatherDataLibrary.DataAccess;
using WeatherDataLibrary.Models;

namespace WeatherDataLibrary
{
    public class Service
    {
        public static void temp()
        {
            DateTime d = new();
            var i = d.Year;
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

        public static List<int> Year(List<Data> datas)
        {
            List<int> YEARS = new List<int>();

            var year = datas
                .GroupBy(y => y.Date.Year);

            foreach (var item in year)
            {
                YEARS.Add(item.Key);
            }
            return YEARS;
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
            try
            {
                var temp = datas
                    .Where(t => t.SensorName == sensorName && t.Date == dateChoice)
                    .Average(t => t.Temp);
                string result = $"{dateChoice.ToShortDateString()}  {Math.Round(temp, 2)}°";
                return result;
            }
            catch (Exception)
            {
                string result = $"Tempereaturdata finns ej för det valda datumet ({dateChoice.ToShortDateString()})";
                return result;
            }
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

        public static string InOrOut(string sensorName)
        {
            ConsoleKey insideOutside = Console.ReadKey().Key;
            Console.Clear();
            switch (insideOutside)
            {
                case ConsoleKey.D1:
                    sensorName = "Inne";
                    break;
                case ConsoleKey.D2:
                    sensorName = "Ute";
                    break;
                default:
                    break;
            }
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
                    tempAverage = l.Average(l => l.Temp),
                    date = l.Key
                });
            var moldList = list
                .Where(m => m.tempAverage >= 0 && m.humidityAverage >= 78)
                .Select(m => new
                {
                    date = m.date,
                    moldCalc = ((m.humidityAverage - 78) * (m.tempAverage / 15)) / 0.22
                });
            foreach (var item in moldList)
            {
                Day d = new Day();
                d.Date = item.date;
                d.MoldRisk = item.moldCalc;
                result.Add(d);
            }
            return result;
        }

        public static List<string> MeteorologicalWinterAutumn(List<Data> datas, List<int> yearCheck, int tempLimit)
        {
            List<string> result = new List<string>();
            foreach (var year in yearCheck)
            {
                string yearlyResult = tempLimit == 0 ? $"Ingen meterologisk vinter inträffade säsongen {year}/{year + 1}" :
                    $"Ingen meterologisk höst inträffade året {year}";
                var seasonCheck = datas
                    .Where(a => a.SensorName == "Ute")
                    .GroupBy(a => a.Date.Date)
                    .OrderBy(a => a.Key)
                    .Where(a => a.Key >= new DateTime(year, 08, 01) && a.Key <= new DateTime(year + 1, 02, 15))
                    .Select(a => new
                    {
                        tempAverage = a.Average(a => a.Temp),
                        date = a.Key
                    })
                    .ToList();

                //MeasuringGapCheck(datas, year);

                int rowCounter = 0;
                int counter = 0;
                foreach (var item in seasonCheck)
                {
                    if (item.tempAverage < tempLimit)
                    {
                        counter++;
                        if (rowCounter > 0 && seasonCheck[rowCounter - 1].date.AddDays(1) == item.date)
                        {
                            if (counter == 5)
                            {
                                yearlyResult = tempLimit == 0 ? $"Meterologisk vinter inträffar {item.date.AddDays(-4).ToShortDateString()}" :
                                    $"Meterologisk höst inträffar {item.date.AddDays(-4).ToShortDateString()}";
                                result.Add(yearlyResult);
                                break;
                            }
                        }
                        else if (rowCounter == 0)
                        { }//Needed for not automatically setting the first index-counter to 0 in the else-section when it's not supposed to.
                        else
                        {
                            counter = 0;
                        }
                    }
                    else
                    {
                        counter = 0;
                    }
                    rowCounter++;
                }
                if (seasonCheck.Count != 0 && (yearlyResult == $"Ingen meterologisk vinter inträffade säsongen {year}/{year + 1}" || yearlyResult == $"Ingen meterologisk höst inträffade året {year}"))
                {
                    result.Add(yearlyResult);
                }
            }
            return result;
        }

        public static void MeasuringGapCheck(List<Data> datas, int year)
        {
            //var gap = datas
            //    .Where(g => g.SensorName == "Ute")
            //    .GroupBy(g => g.Date.Date)
            //    .OrderBy(g => g.Key)
            //    .Where(g => g.Key >= new DateTime(year, 08, 01) && g.Key <= new DateTime(year + 1, 02, 15))
            //    .Where(g => )
            //    .Select(g => 
        }
    }
}
