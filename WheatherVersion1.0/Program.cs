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
            //DbService.FixDb(); //Setting up the database

            Console.WriteLine("Laddar in databas i lokal lista. . .");
            List<Data> datas = DbService.DataBase();

            ConsoleAppMenu.Menu(datas);
        }
    }
}
