﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherDataLibrary.DataAccess;

namespace WeatherDataLibrary.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210108122041_try5")]
    partial class try5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("WeatherDataLibrary.Models.Data", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Humidity")
                        .HasColumnType("int");

                    b.Property<int?>("SensorId")
                        .HasColumnType("int");

                    b.Property<string>("SensorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Temp")
                        .HasColumnType("float");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("Data");
                });

            modelBuilder.Entity("WeatherDataLibrary.Models.Sensor", b =>
                {
                    b.Property<int>("SensorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("SensorName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SensorId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("WeatherDataLibrary.Models.Data", b =>
                {
                    b.HasOne("WeatherDataLibrary.Models.Sensor", null)
                        .WithMany("WeatherData")
                        .HasForeignKey("SensorId");
                });

            modelBuilder.Entity("WeatherDataLibrary.Models.Sensor", b =>
                {
                    b.Navigation("WeatherData");
                });
#pragma warning restore 612, 618
        }
    }
}
