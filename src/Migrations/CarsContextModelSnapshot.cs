﻿// <auto-generated />
using System;
using AvtoNetScraper.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AvtoNetScraper.Migrations
{
    [DbContext(typeof(CarsContext))]
    partial class CarsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4");

            modelBuilder.Entity("AvtoNetScraper.Database.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdLocation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Age")
                        .HasColumnType("TEXT");

                    b.Property<string>("BodyShape")
                        .HasColumnType("TEXT");

                    b.Property<string>("CO2Emissions")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChassisNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("CityConsumption")
                        .HasColumnType("TEXT");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<string>("CombinedConsumption")
                        .HasColumnType("TEXT");

                    b.Property<string>("Details")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DoorNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmissionClass")
                        .HasColumnType("TEXT");

                    b.Property<string>("Engine")
                        .HasColumnType("TEXT");

                    b.Property<string>("EngineType")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("FirstRegistration")
                        .HasColumnType("TEXT");

                    b.Property<string>("Interior")
                        .HasColumnType("TEXT");

                    b.Property<string>("InternalNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("LocalPicturePath")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MileageInKm")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("OutOfTownConsumption")
                        .HasColumnType("TEXT");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProductionYear")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StockStatus")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("TechnicalInspectionValidUntil")
                        .HasColumnType("TEXT");

                    b.Property<string>("Transmission")
                        .HasColumnType("TEXT");

                    b.Property<int>("UrlId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("AvtoNetScraper.Database.NotificationLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CarId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SentTimestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NotificationsLog");
                });

            modelBuilder.Entity("AvtoNetScraper.Database.Url", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Urls");
                });
#pragma warning restore 612, 618
        }
    }
}
