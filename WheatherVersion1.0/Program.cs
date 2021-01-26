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
            Console.WriteLine("Laddar in databas i lokal lista. . .");
            List<Data> workingDB = DbService.DataBase();

            ConsoleAppMenu.Menu(workingDB);
        }
    }
}
