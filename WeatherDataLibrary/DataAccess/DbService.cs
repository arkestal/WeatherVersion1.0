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
                Console.WriteLine("Lägger till databas");

                db.AddRange(sensors);
                Console.WriteLine("Klar!");
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

        public static void AverageTempCase(List<Data> datas, string inOrOutQuestion, string sensorName)
        {
            //string sensorName = "default";
            do
            {
                Console.Write(inOrOutQuestion);
                sensorName = Service.InOrOut(sensorName);
                if (sensorName == "default")
                {
                    Console.WriteLine("\n\tOgiltig inmatning!");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            } while (sensorName == "default");
            DateTime dateChoice;
            bool loopRunning = true;
            do
            {
                Console.Clear();
                Console.Write("\n\tSkriv in datum enligt formatet [ÅÅÅÅ-MM-DD] för att se medeltemperaturen för det datumet." +
                    "\n\tSkriv [EXIT] för att återgå till gästmenyn" +
                    "\n\tVälj: ");

                string input = Console.ReadLine();
                Console.Clear();
                if (input.ToUpper() == "EXIT")
                {
                    Console.WriteLine("\n\tÅtergår till gästmenyn. . .");
                    loopRunning = false;
                    Thread.Sleep(1000);
                }
                else
                {
                    bool check = DateTime.TryParse(input, out dateChoice);
                    Console.Clear();
                    if (check)
                    {
                        string result = Service.AverageTemp(datas, dateChoice, sensorName);
                        Console.Write($"\n\tMedeltemperatur {sensorName.ToUpper()}\n\n\t{result}\n");
                        Console.WriteLine("\n\tTryck på valfri tangent för att återgå till gästmenyn");
                        loopRunning = false;
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("\n\tOgiltig inmatning!");
                        Thread.Sleep(1500);
                    }
                }
            } while (loopRunning);
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

        public static void WarmColdCase(List<Data> datas, string inOrOutQuestion, string sensorName)
        {
            //string sensorName = "default";
            ConsoleKey orderChoice;
            string tempOrder = "fallande";
            Console.WriteLine("\n\tMedeltemperatur");
            do
            {
                Console.Write(inOrOutQuestion);
                sensorName = Service.InOrOut(sensorName);
                if (sensorName == "default")
                {
                    Console.WriteLine("\n\tOgiltig inmatning!");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            } while (sensorName == "default");
            List<Day> tempList = new List<Day>();
            tempList = Service.WarmColdSort(datas, sensorName);
            tempList = tempList
                .OrderByDescending(t => t.Temp)
                .ToList();
            do
            {
                Console.Clear();
                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Medeltemperatur({tempOrder})\tTopp 10\n");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"{i + 1}\t|{tempList[i].Date.ToShortDateString()}     |{Math.Round(tempList[i].Temp, 2)}°");
                }
                Console.WriteLine("\n\t[ENTER] Ivertera listan\n\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                orderChoice = Console.ReadKey().Key;
                switch (orderChoice)
                {
                    case ConsoleKey.Enter:
                        if (tempOrder == "fallande")
                        {
                            tempList = tempList
                                .OrderBy(t => t.Temp)
                                .ToList();
                            tempOrder = "stigande";
                        }
                        else
                        {
                            tempList = tempList
                                .OrderByDescending(t => t.Temp)
                                .ToList();
                            tempOrder = "fallande";
                        }
                        break;
                    default:
                        break;
                }
            } while (orderChoice == ConsoleKey.Enter);
        }

        public static int GetHumidity(string row)
        {
            int index = 0;

            string[] split = row.Split(' ', ',', ',', ',');

            index = int.Parse(split[4]);

            return index;
        }

        public static void DryWetCase(List<Data> datas, string inOrOutQuestion, string sensorName)
        {
            //string sensorName = "default";
            ConsoleKey orderChoice;
            string humidityOrder = "fallande";
            Console.WriteLine("\n\tMedelluftfuktighet");
            do
            {
                Console.Write(inOrOutQuestion);
                sensorName = Service.InOrOut(sensorName);
                if (sensorName == "default")
                {
                    Console.WriteLine("\n\tOgiltig inmatning!");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            } while (sensorName == "default");
            List<Day> humidityList = new List<Day>();
            humidityList = Service.WetDrySort(datas, sensorName);
            humidityList = humidityList
                .OrderByDescending(h => h.Humidity)
                .ToList();
            do
            {
                Console.Clear();
                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Medelluftfuktighet({humidityOrder})\tTopp 10\n");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"{i + 1}\t|{humidityList[i].Date.ToShortDateString()}     |{humidityList[i].Humidity} %");

                }
                Console.WriteLine("\n\t[ENTER] Ivertera listan\n\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                orderChoice = Console.ReadKey().Key;
                switch (orderChoice)
                {
                    case ConsoleKey.Enter:
                        if (humidityOrder == "fallande")
                        {
                            humidityList = humidityList
                                .OrderBy(t => t.Humidity)
                                .ToList();
                            humidityOrder = "stigande";
                        }
                        else
                        {
                            humidityList = humidityList
                                .OrderByDescending(t => t.Humidity)
                                .ToList();
                            humidityOrder = "fallande";
                        }
                        break;
                }
            } while (orderChoice == ConsoleKey.Enter);
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

        public static void MoldRiskCase(List<Data> datas, string inOrOutQuestion, string sensorName)
        {
            int dayCounter = Service.NumberOfDays(datas);
            //string sensorName = "default";
            int counter = 0;
            ConsoleKey orderChoice;
            string moldRiskOrder = "fallande";
            Console.WriteLine("\n\tMögelrisk");
            do
            {
                Console.Write(inOrOutQuestion);
                sensorName = Service.InOrOut(sensorName);
                if (sensorName == "default")
                {
                    Console.WriteLine("\n\tOgiltig inmatning!");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            } while (sensorName == "default");
            List<Day> moldRiskList = new List<Day>();
            moldRiskList = Service.MoldRiskSort(datas, sensorName);
            moldRiskList = moldRiskList
                .OrderByDescending(h => h.MoldRisk)
                .ToList();
            do
            {
                Console.Clear();
                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Mögelrisk({moldRiskOrder})\n");
                foreach (var item in moldRiskList)
                {
                    counter++;
                    Console.WriteLine($"{counter}\t|{item.Date.ToShortDateString()}     |{Math.Round(item.MoldRisk, 2)} %");
                }

                if (counter == 0)
                {
                    Console.WriteLine("\n\tIngen mögelrisk att rapportera!");
                }
                else
                {
                    Console.WriteLine($"\n\tResterande {dayCounter - counter} dagar löper ingen risk för mögel.");
                }
                Console.WriteLine("\n\t[ENTER] Ivertera listan\n\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                orderChoice = Console.ReadKey().Key;
                switch (orderChoice)
                {
                    case ConsoleKey.Enter:
                        counter = 0;
                        if (moldRiskOrder == "fallande")
                        {
                            moldRiskList = moldRiskList
                                .OrderBy(m => m.MoldRisk)
                                .ToList();
                            moldRiskOrder = "stigande";
                        }
                        else
                        {
                            moldRiskList = moldRiskList
                                .OrderByDescending(m => m.MoldRisk)
                                .ToList();
                            moldRiskOrder = "fallande";
                        }
                        break;
                }
            } while (orderChoice == ConsoleKey.Enter);
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

        public static void AutumnCase(List<Data> datas)
        {
            List<int> yearCheck = Service.Year(datas);
            DbService.AutumnCase(datas);
            Console.WriteLine("\n\tMeterologisk höst");
            List<string> autumnDate = Service.MeteorologicalWinterAutumn(datas, yearCheck, 10);
            foreach (var item in autumnDate)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("\n\n\tTryck på valfri tangent för att återgå till gästmenyn");
            Console.ReadKey();
        }

        public static void WinterCase(List<Data> datas)
        {
            List<int> yearCheck = Service.Year(datas);
            Console.WriteLine("\n\tMeterologisk vinter");
            List<string> winterDate = Service.MeteorologicalWinterAutumn(datas, yearCheck, 0);
            foreach (var item in winterDate)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("\n\n\tTryck på valfri tangent för att återgå till gästmenyn");
            Console.ReadKey();
        }
    }
}
