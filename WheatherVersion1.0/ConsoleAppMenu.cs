using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WeatherDataLibrary;
using WeatherDataLibrary.Models;

namespace WeatherVersion1._0
{
    public class ConsoleAppMenu
    {
        public static void GuestMenu(List<Data> datas)
        {
            string inOrOutQuestion = "\n\t[1] Inne\n\t[2] Ute\n\tVälj: ";
            int resultAmount = 10;
            bool isRunning = true;
            while (isRunning)
            {
                string sensorName = "default";
                Console.Clear();
                Console.Write("\n\tVäderstationen" +
                    "\n\n\t[1] Sök medeltemperatur för valt datum" +
                    "\n\t[2] Sortering av varmast/kallast medeltemperatur under en dag" +
                    "\n\t[3] Sortering av torraste/fuktigaste medelluftfuktighet" +
                    "\n\t[4] Sortering av minst/störst risk för mögel" +
                    "\n\t[5] Datum för meterologisk höst" +
                    "\n\t[6] Datum för meterologisk vinter" +
                    "\n\t[7] Avsluta" +
                    "\n\tVälj: ");

                ConsoleKey key = Console.ReadKey().Key;
                Console.Clear();
                switch (key)
                {
                    case ConsoleKey.D1:
                        AverageTempCase(datas, inOrOutQuestion, sensorName);
                        break;
                    case ConsoleKey.D2:
                        WarmColdCase(datas, inOrOutQuestion, sensorName, resultAmount);
                        break;
                    case ConsoleKey.D3:
                        DryWetCase(datas, inOrOutQuestion, sensorName, resultAmount);
                        break;
                    case ConsoleKey.D4:
                        MoldRiskCase(datas, inOrOutQuestion, sensorName, resultAmount);
                        break;
                    case ConsoleKey.D5:
                        AutumnCase(datas);
                        break;
                    case ConsoleKey.D6:
                        WinterCase(datas);
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

        public static void AverageTempCase(List<Data> datas, string inOrOutQuestion, string sensorName)
        {
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
        public static void WarmColdCase(List<Data> datas, string inOrOutQuestion, string sensorName, int resultAmount)
        {
            string order = "Topp 10";
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
                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Medeltemperatur({tempOrder})\t{order}\n");
                for (int i = 0; i < resultAmount; i++)
                {
                    Console.WriteLine($"{i + 1}\t|{tempList[i].Date.ToShortDateString()}     |{Math.Round(tempList[i].Temp, 2)}°C");
                }
                if (resultAmount == tempList.Count())
                {
                    Console.WriteLine("\n\t[ENTER]\tIvertera listan\n\t[A]\tVisa endast topp 10\n\t\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                }
                else
                {
                    Console.WriteLine("\n\t[ENTER]\tIvertera listan\n\t[A]\tVisa fullständig lista\n\t\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                }
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
                    case ConsoleKey.A:
                        if (resultAmount == 10)
                        {
                            resultAmount = tempList.Count();
                            order = "Fullständig lista";
                        }
                        else
                        {
                            resultAmount = 10;
                            order = "Topp 10";
                        }
                        break;
                    default:
                        break;
                }
            } while (orderChoice == ConsoleKey.Enter || orderChoice == ConsoleKey.A);
        }
        public static void DryWetCase(List<Data> datas, string inOrOutQuestion, string sensorName, int resultAmount)
        {
            string order = "Topp 10";
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
                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Medelluftfuktighet({humidityOrder})\t{order}\n");
                for (int i = 0; i < resultAmount; i++)
                {
                    Console.WriteLine($"{i + 1}\t|{humidityList[i].Date.ToShortDateString()}     |{humidityList[i].Humidity} %");
                }
                if (resultAmount == humidityList.Count())
                {
                    Console.WriteLine("\n\t[ENTER]\tIvertera listan\n\t[A]\tVisa endast topp 10\n\t\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                }
                else
                {
                    Console.WriteLine("\n\t[ENTER]\tIvertera listan\n\t[A]\tVisa fullständig lista\n\t\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                }
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
                    case ConsoleKey.A:
                        if (resultAmount == 10)
                        {
                            resultAmount = humidityList.Count();
                            order = "Fullständig lista";
                        }
                        else
                        {
                            resultAmount = 10;
                            order = "Topp 10";
                        }
                        break;
                }
            } while (orderChoice == ConsoleKey.Enter || orderChoice == ConsoleKey.A);
        }
        public static void MoldRiskCase(List<Data> datas, string inOrOutQuestion, string sensorName, int resultAmount)
        {
            string order = "Topp 10";
            int dayCounter = Service.NumberOfDays(datas);
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
                int counter = 0;
                Console.Clear();
                Console.WriteLine($"\n{sensorName.ToUpper()}\tDatum        Mögelrisk({moldRiskOrder})\t{order}\n");
                for (int i = 0; i < resultAmount; i++)
                {
                    counter++;
                    Console.WriteLine($"{i + 1}\t|{moldRiskList[i].Date.ToShortDateString()}     |{Math.Round(moldRiskList[i].MoldRisk, 2)} %");
                }
                if (counter == 0)
                {
                    Console.WriteLine("\n\tIngen mögelrisk att rapportera!");
                }
                else
                {
                    Console.WriteLine($"\n\t{dayCounter - moldRiskList.Count()} dagar löper ingen risk för mögel.");
                }
                if (resultAmount == moldRiskList.Count())
                {
                    Console.WriteLine("\n\t[ENTER]\tIvertera listan\n\t[A]\tVisa endast topp 10\n\t\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                }
                else
                {
                    Console.WriteLine("\n\t[ENTER]\tIvertera listan\n\t[A]\tVisa fullständig lista\n\t\tTryck annars på valfri annan tangent för att återgå till gästmenyn");
                }
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
                    case ConsoleKey.A:
                        if (resultAmount == 10)
                        {
                            resultAmount = moldRiskList.Count();
                            order = "Fullständig lista";
                        }
                        else
                        {
                            resultAmount = 10;
                            order = "Topp 10";
                        }
                        break;
                }
            } while (orderChoice == ConsoleKey.Enter || orderChoice == ConsoleKey.A);
        }
        public static void AutumnCase(List<Data> datas)
        {
            List<int> yearCheck = Service.Year(datas);
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
