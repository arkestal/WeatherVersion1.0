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
            List<Data> workingDB = DbService.DataBase();

            GuestMenu(workingDB);
        }

        private static void GuestMenu(List<Data> datas)
        {
            string inOrOutQuestion = "\n\t[1] Inne\n\t[2] Ute\n\tVälj: ";
            bool isRunning = true;
            while (isRunning)
            {
                string sensorName = "default";
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
                        DbService.AverageTempCase(datas, inOrOutQuestion, sensorName);
                        //do
                        //{
                        //    Console.Write(inOrOutQuestion);
                        //    sensorName = Service.InOrOut(sensorName);
                        //    if (sensorName == "default")
                        //    {
                        //        Console.WriteLine("\n\tOgiltig inmatning!");
                        //        Thread.Sleep(1000);
                        //        Console.Clear();
                        //    }
                        //} while (sensorName == "default");
                        //DateTime dateChoice;
                        //bool loopRunning = true;
                        //do
                        //{
                        //    Console.Clear();
                        //    Console.Write("\n\tSkriv in datum enligt formatet [ÅÅÅÅ-MM-DD] för att se medeltemperaturen för det datumet." +
                        //        "\n\tSkriv [EXIT] för att återgå till gästmenyn" +
                        //        "\n\tVälj: ");

                        //    string input = Console.ReadLine();
                        //    Console.Clear();
                        //    if (input.ToUpper() == "EXIT")
                        //    {
                        //        Console.WriteLine("\n\tÅtergår till gästmenyn. . .");
                        //        loopRunning = false;
                        //        Thread.Sleep(1000);
                        //    }
                        //    else
                        //    {
                        //        bool check = DateTime.TryParse(input, out dateChoice);
                        //        Console.Clear();
                        //        if (check)
                        //        {
                        //            string result = Service.AverageTemp(datas, dateChoice, sensorName);
                        //            Console.Write($"\n\tMedeltemperatur {sensorName.ToUpper()}\n\n\t{result}\n");
                        //            Console.WriteLine("\n\tTryck på valfri tangent för att återgå till gästmenyn");
                        //            loopRunning = false;
                        //            Console.ReadKey();
                        //        }
                        //        else
                        //        {
                        //            Console.WriteLine("\n\tOgiltig inmatning!");
                        //            Thread.Sleep(1500);
                        //        }
                        //    }
                        //} while (loopRunning);
                        break;
                    case ConsoleKey.D2:
                        DbService.WarmColdCase(datas, inOrOutQuestion, sensorName);
                        //int counter = 0;
                        //string tempOrder = "fallande";
                        //Console.WriteLine("\n\tMedeltemperatur");
                        //do
                        //{
                        //    Console.Write(inOrOutQuestion);
                        //    sensorName = Service.InOrOut(sensorName);
                        //    if (sensorName == "default")
                        //    {
                        //        Console.WriteLine("\n\tOgiltig inmatning!");
                        //        Thread.Sleep(1000);
                        //        Console.Clear();
                        //    }
                        //} while (sensorName == "default");
                        //List<Day> tempList = new List<Day>();
                        //tempList = Service.WarmColdSort(datas, sensorName);
                        //tempList = tempList
                        //    .OrderByDescending(t => t.Temp)
                        //    .ToList();
                        //do
                        //{
                        //    Console.Clear();
                        //    Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Medeltemperatur({tempOrder})\tTopp 10\n");
                        //    for (int i = 0; i < 10; i++)
                        //    {
                        //        Console.WriteLine($"{i + 1}\t|{tempList[i].Date.ToShortDateString()}     |{Math.Round(tempList[i].Temp, 2)}°");
                        //    }
                        //    Console.WriteLine("\n\t[ENTER] Ivertera listan\n\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                        //    orderChoice = Console.ReadKey().Key;
                        //    switch (orderChoice)
                        //    {
                        //        case ConsoleKey.Enter:
                        //            counter = 0;
                        //            if (tempOrder == "fallande")
                        //            {
                        //                tempList = tempList
                        //                    .OrderBy(t => t.Temp)
                        //                    .ToList();
                        //                tempOrder = "stigande";
                        //            }
                        //            else
                        //            {
                        //                tempList = tempList
                        //                    .OrderByDescending(t => t.Temp)
                        //                    .ToList();
                        //                tempOrder = "fallande";
                        //            }
                        //            break;
                        //        default:
                        //            break;
                        //    }
                        //} while (orderChoice == ConsoleKey.Enter);
                        break;
                    case ConsoleKey.D3:
                        DbService.DryWetCase(datas, inOrOutQuestion, sensorName);
                        //int counter = 0;
                        //string humidityOrder = "fallande";
                        //Console.WriteLine("\n\tMedelluftfuktighet");
                        //do
                        //{
                        //    Console.Write(inOrOutQuestion);
                        //    sensorName = Service.InOrOut(sensorName);
                        //    if (sensorName == "default")
                        //    {
                        //        Console.WriteLine("\n\tOgiltig inmatning!");
                        //        Thread.Sleep(1000);
                        //        Console.Clear();
                        //    }
                        //} while (sensorName == "default");
                        //List<Day> humidityList = new List<Day>();
                        //humidityList = Service.WetDrySort(datas, sensorName);
                        //humidityList = humidityList
                        //    .OrderByDescending(h => h.Humidity)
                        //    .ToList();
                        //do
                        //{
                        //    Console.Clear();
                        //    Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Medelluftfuktighet({humidityOrder})\tTopp 10\n");
                        //    for (int i = 0; i < 10; i++)
                        //    {
                        //        Console.WriteLine($"{i + 1}\t|{humidityList[i].Date.ToShortDateString()}     |{humidityList[i].Humidity} %");

                        //    }
                        //    Console.WriteLine("\n\t[ENTER] Ivertera listan\n\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                        //    orderChoice = Console.ReadKey().Key;
                        //    switch (orderChoice)
                        //    {
                        //        case ConsoleKey.Enter:
                        //            counter = 0;
                        //            if (humidityOrder == "fallande")
                        //            {
                        //                humidityList = humidityList
                        //                    .OrderBy(t => t.Humidity)
                        //                    .ToList();
                        //                humidityOrder = "stigande";
                        //            }
                        //            else
                        //            {
                        //                humidityList = humidityList
                        //                    .OrderByDescending(t => t.Humidity)
                        //                    .ToList();
                        //                humidityOrder = "fallande";
                        //            }
                        //            break;
                        //    }
                        //} while (orderChoice == ConsoleKey.Enter);
                        break;
                    case ConsoleKey.D4:
                        DbService.MoldRiskCase(datas, inOrOutQuestion, sensorName);
                        //int counter = 0;
                        //string moldRiskOrder = "fallande";
                        //Console.WriteLine("\n\tMögelrisk");
                        //do
                        //{
                        //    Console.Write(inOrOutQuestion);
                        //    sensorName = Service.InOrOut(sensorName);
                        //    if (sensorName == "default")
                        //    {
                        //        Console.WriteLine("\n\tOgiltig inmatning!");
                        //        Thread.Sleep(1000);
                        //        Console.Clear();
                        //    }
                        //} while (sensorName == "default");
                        //List<Day> moldRiskList = new List<Day>();
                        //moldRiskList = Service.MoldRiskSort(datas, sensorName);
                        //moldRiskList = moldRiskList
                        //    .OrderByDescending(h => h.MoldRisk)
                        //    .ToList();
                        //do
                        //{
                        //    Console.Clear();
                        //    Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Mögelrisk({moldRiskOrder})\n");
                        //    foreach (var item in moldRiskList)
                        //    {
                        //        counter++;
                        //        Console.WriteLine($"{counter}\t|{item.Date.ToShortDateString()}     |{Math.Round(item.MoldRisk, 2)} %");
                        //    }

                        //    if (counter == 0)
                        //    {
                        //        Console.WriteLine("\n\tIngen mögelrisk att rapportera!");
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine($"\n\tResterande {dayCounter - counter} dagar löper ingen risk för mögel.");
                        //    }
                        //    Console.WriteLine("\n\t[ENTER] Ivertera listan\n\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                        //    orderChoice = Console.ReadKey().Key;
                        //    switch (orderChoice)
                        //    {
                        //        case ConsoleKey.Enter:
                        //            counter = 0;
                        //            if (moldRiskOrder == "fallande")
                        //            {
                        //                moldRiskList = moldRiskList
                        //                    .OrderBy(m => m.MoldRisk)
                        //                    .ToList();
                        //                moldRiskOrder = "stigande";
                        //            }
                        //            else
                        //            {
                        //                moldRiskList = moldRiskList
                        //                    .OrderByDescending(m => m.MoldRisk)
                        //                    .ToList();
                        //                moldRiskOrder = "fallande";
                        //            }
                        //            break;
                        //    }
                        //} while (orderChoice == ConsoleKey.Enter);
                        break;
                    case ConsoleKey.D5:
                        DbService.AutumnCase(datas);
                        //Console.WriteLine("\n\tMeterologisk höst");
                        //List<string> autumnDate = Service.MeteorologicalWinterAutumn(datas, yearCheck, 10);
                        //foreach (var item in autumnDate)
                        //{
                        //    Console.WriteLine(item);
                        //}
                        //Console.WriteLine("\n\n\tTryck på valfri tangent för att återgå till gästmenyn");
                        //Console.ReadKey();
                        break;
                    case ConsoleKey.D6:
                        DbService.WinterCase(datas);
                        //Console.WriteLine("\n\tMeterologisk vinter");
                        //List<string> winterDate = Service.MeteorologicalWinterAutumn(datas, yearCheck, 0);
                        //foreach (var item in winterDate)
                        //{
                        //    Console.WriteLine(item);
                        //}
                        //Console.WriteLine("\n\n\tTryck på valfri tangent för att återgå till gästmenyn");
                        //Console.ReadKey();
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
    }
}
