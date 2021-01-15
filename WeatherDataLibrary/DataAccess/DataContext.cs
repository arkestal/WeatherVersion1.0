using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherDataLibrary.DataAccess
{
    public class DataContext : DbContext
    {
        private string connectionString;

        public DataContext()
            : base()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appSettings.json", optional: false);

            var configuration = builder.Build();

            connectionString = configuration.GetConnectionString("sqlConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Models.Sensor> Sensors { get; set; }
        public DbSet<Models.Data> Datas { get; set; }
    }
}
