using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WeatherDataLibrary;
using WeatherDataLibrary.DataAccess;
using WeatherDataLibrary.Models;

namespace WeatherVersion1._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Laddar in databas i lokal lista");
            List<Data> workingDB = Service.DataBase();
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "TemperaturData.csv");
            //string[] data = File.ReadAllLines(path);

            //List<Data> weatherData = GetAllData(data);
            //List<Sensor> sensors = GetSensor(data);
            //foreach (var sensor in sensors)
            //{
            //    sensor.WeatherData = new List<Data>();
            //}
            //foreach (var item in weatherData)
            //{
            //    foreach (var sensor in sensors)
            //    {
            //        if (item.SensorName == sensor.SensorName)
            //        {
            //            sensor.WeatherData.Add(item);
            //        }

            //    }
            //}

            //LoadToDatabase(sensors);
            GuestMenu(workingDB);
            //DateTime choice = DateTime.Parse(Console.ReadLine());
            //Service.AverageTemp(choice, "ute");
        }

        private static void GuestMenu(List<Data> datas)
        {
            int dayCounter = Service.NumberOfDays(datas);
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.Write("\n\tVäderstationens gästmeny" +
                    "\n\n\t[1] Sök medeltemperatur för valt datum" +
                    "\n\t[2] Sortering av varmast/kallast medeltemperatur under en dag" +
                    "\n\t[3] Sortering av torraste/fuktigaste medelluftfuktighet" +
                    "\n\t[4] Sortering av minst/störst risk för mögel" +
                    "\n\t[5] Datum för meterologisk höst" +
                    "\n\t[6] Datum för meterologisk vinter" +
                    "\n\t[7] Tillbaka till huvudmenyn" +
                    "\n\tVälj: ");

                ConsoleKey key = Console.ReadKey().Key;
                Console.Clear();
                switch (key)
                {
                    case ConsoleKey.D1:
                        string sensorName = Service.InOrOut();
                        Console.Write("\n\tSkriv in datum enligt formatet [ÅÅÅÅ-MM-DD] för att se medeltemperaturen för det datumet." +
                            "\n\tSkriv [EXIT] för att att avbryta" +
                            "\n\tVälj: ");
                        try
                        {
                            DateTime dateChoice = DateTime.Parse(Console.ReadLine());
                            Console.Clear();
                            string result = Service.AverageTemp(datas, dateChoice, sensorName);
                            Console.Write($"\n\tMedeltemperatur {sensorName.ToUpper()}\n\n\t{result}°\n");
                            Console.WriteLine("\n\tTryck på valfri tangent för att återgå till gästmenyn");
                            Console.ReadKey();
                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            Console.WriteLine("Återgår till gästmenyn. . .");
                            Thread.Sleep(1500);
                        }
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("\n\tMedeltemperatur");
                        sensorName = Service.InOrOut();
                        int counter = 0;
                        Console.Write("\n\t[1] Varmast till kallast medeltemperatur" +
                            "\n\t[2] Kallast till varmast medeltemperatur" +
                            "\n\t[3] Återgå till gästmenyn" +
                            "\n\tVälj: ");
                        ConsoleKey warmCold = Console.ReadKey().Key;
                        Console.Clear();
                        List<Day> tempList = new List<Day>();
                        switch (warmCold)
                        {
                            case ConsoleKey.D1:
                                tempList = Service.WarmColdSort(datas, sensorName);
                                tempList = tempList
                                    .OrderByDescending(t => t.Temp)
                                    .ToList();
                                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Temperatur(fallande)\n");
                                foreach (var item in tempList)
                                {
                                    counter++;
                                    Console.WriteLine($"{counter}\t|{item.Date.ToShortDateString()}     |{Math.Round(item.Temp, 2)}°");
                                }
                                Console.WriteLine("\nTryck på valfri tangent för att återgå till gästmenyn");
                                Console.ReadKey();
                                break;
                            case ConsoleKey.D2:
                                tempList = Service.WarmColdSort(datas, sensorName);
                                tempList = tempList
                                    .OrderBy(t => t.Temp)
                                    .ToList();
                                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Temperatur(stigande)\n");
                                foreach (var item in tempList)
                                {
                                    counter++;
                                    Console.WriteLine($"{counter}\t|{item.Date.ToShortDateString()}     |{Math.Round(item.Temp, 2)}°");
                                }
                                Console.WriteLine("\nTryck på valfri tangent för att återgå till gästmenyn");
                                Console.ReadKey();
                                break;
                            default:
                                Console.WriteLine("Återgår till gästmenyn. . .");
                                Thread.Sleep(1500);
                                break;
                        }
                        break;
                    case ConsoleKey.D3:
                        Console.WriteLine("\n\tMedelluftfuktighet");
                        sensorName = Service.InOrOut();
                        counter = 0;
                        Console.Write("\n\t[1] Torrast till fuktigast medelluftfuktighet" +
                            "\n\t[2] Fuktigast till torrast medelluftfuktighet" +
                            "\n\t[3] Återgå till gästmenyn" +
                            "\n\tVälj: ");
                        ConsoleKey wetDry = Console.ReadKey().Key;
                        Console.Clear();
                        List<Day> humidityList = new List<Day>();
                        switch (wetDry)
                        {
                            case ConsoleKey.D1:
                                humidityList = Service.WetDrySort(datas, sensorName);
                                humidityList = humidityList
                                    .OrderBy(h => h.Humidity)
                                    .ToList();
                                Console.WriteLine($"\n{ sensorName.ToUpper()}\tDatum        Medelluftfuktighet(stigande)\n");
                                foreach (var item in humidityList)
                                {
                                    counter++;
                                    Console.WriteLine($"{counter}\t|{item.Date.ToShortDateString()}     |{item.Humidity} %");
                                }
                                Console.WriteLine("\nTryck på valfri tangent för att återgå till gästmenyn");
                                Console.ReadKey();
                                break;
                            case ConsoleKey.D2:
                                humidityList = Service.WetDrySort(datas, sensorName);
                                humidityList = humidityList
                                    .OrderByDescending(h => h.Humidity)
                                    .ToList();
                                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Medelluftfuktighet(fallande)\n");
                                foreach (var item in humidityList)
                                {
                                    counter++;
                                    Console.WriteLine($"{counter}\t|{item.Date.ToShortDateString()}     |{item.Humidity} %");
                                }
                                Console.WriteLine("\nTryck på valfri tangent för att återgå till gästmenyn");
                                Console.ReadKey();
                                break;
                            default:
                                break;
                        }
                        break;
                    case ConsoleKey.D4:
                        Console.WriteLine("\n\tMögelrisk");
                        sensorName = Service.InOrOut();
                        counter = 0;
                        Console.Write("\n\t[1] Minst till störst risk för mögel" +
                            "\n\t[2] Störst till minst risk för mögel" +
                            "\n\t[3] Återgå till gästmenyn" +
                            "Välj: ");
                        ConsoleKey moldRisk = Console.ReadKey().Key;
                        Console.Clear();
                        List<Day> moldRiskList = new List<Day>();
                        switch (moldRisk)
                        {
                            case ConsoleKey.D1:
                                moldRiskList = Service.MoldRiskSort(datas, sensorName);
                                moldRiskList = moldRiskList
                                    .OrderBy(m => m.MoldRisk)
                                    .ToList();
                                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Risk för mögel(stigande)\n");
                                foreach (var item in moldRiskList)
                                {
                                    counter++;
                                    Console.WriteLine($"{counter}\t|{item.Date.ToShortDateString()}     |{Math.Round(item.MoldRisk, 2)} %");
                                }
                                break;
                            case ConsoleKey.D2:
                                moldRiskList = Service.MoldRiskSort(datas, sensorName);
                                moldRiskList = moldRiskList
                                    .OrderByDescending(m => m.MoldRisk)
                                    .ToList();
                                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Risk för mögel(fallande)\n");
                                foreach (var item in moldRiskList)
                                {
                                    counter++;
                                    Console.WriteLine($"{counter}\t|{item.Date.ToShortDateString()}     |{Math.Round(item.MoldRisk, 2)} %");
                                }
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine($"\n\tResterande {dayCounter - counter} dagar löper ingen risk för mögel.");
                        Console.WriteLine("\n\tTryck på valfri tangent för att återgå till gästmenyn");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D5:

                        break;
                    case ConsoleKey.D6:

                        break;
                    case ConsoleKey.D7:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltig inmatning!");
                        Thread.Sleep(1500);
                        break;
                }
            }

        }



        private static void LoadToDatabase(List<Sensor> sensors)
        {
            using (var db = new DataContext())
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
