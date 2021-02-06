﻿using AvtoNetScraper.Database;
using AvtoNetScraper.Scrapers;
using AvtoNetScraper.Settings;
using AvtoNetScraper.Utilities;
using AvtoNetScraper.Notifications;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AvtoNetScraper
{
    internal class Program
    {
        private static IConfiguration Configuration => new ConfigurationBuilder()
        .AddJsonFile("./Settings/appsettings.json", optional: false)
        .Build();

        private static CarsDbHelper _dbHelper = new CarsDbHelper();
        private static void Main(string[] args)
        {
            var settings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            
            if (args.Length > 4)
            {
                Colorful.Console.WriteLine("Invalid arguments supplied. Allowed: -urls, -cars, -images, -notify.", Color.Red);
                return;
            }

            // registers windows-1250 encoding 
            EncodingProvider provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);

            if (args.Length == 0)
            {
                Colorful.Console.WriteLine("Default configuration will be used, scraping car urls first and then car info.", Color.Yellow);
                ScrapeCarUrls(settings);
                ScrapeCarInfo(settings);
            }
            
            if (args.Contains("-urls"))
            {
                Colorful.Console.WriteLine("-urls option chosen, will scrape car urls from appsettings url values.", Color.Yellow);
                ScrapeCarUrls(settings);
            }
            if (args.Contains("-cars"))
            {
                Colorful.Console.WriteLine("-cars option chosen, will scrape car info for all urls in database.", Color.Yellow);
                ScrapeCarInfo(settings);
            }
            if (args.Contains("-images"))
            {
                Colorful.Console.WriteLine("-images option chosen, will download car ad picture for all urls in database.", Color.Yellow);
                DownloadCarImages(settings);
            }
            if (args.Contains("-notify"))
            {
                Colorful.Console.WriteLine("-notify option chosen, will send notifications for every car found.", Color.Yellow);
                SendNotifications(settings);
            }
        }

        private static void SendNotifications(AppSettings settings)
        {
            var carsWithoutNotification = _dbHelper.GetCarsWithoutNotification();
            var urls = _dbHelper.GetUrls(carsWithoutNotification);
            
            var notifer = new TelegramNotifier(settings);
            
            notifer.SendNotificationsForCarsAsync(carsWithoutNotification, urls).Wait();

            var logRecords = carsWithoutNotification.Select(c => 
                new NotificationLog{
                    CarId = c.Id,
                    SentTimestamp = DateTime.UtcNow
                }).ToList();

            _dbHelper.InsertNotificationsLog(logRecords);
        }

        private static void ScrapeCarUrls(AppSettings settings)
        {
            foreach (var url in settings.SearchFilterUrls)
            {
                var scraper = new PageUrlScraper(url, TimeSpan.FromMilliseconds(settings.RequestIntervalMs));
                var urls = scraper.GetCarUrls();
                Colorful.Console.WriteLine($"Finished scraping car urls, got {urls.Count} urls.", Color.GreenYellow);
                
                _dbHelper.MergeUrls(urls);
                Colorful.Console.WriteLine($"Merged {urls.Count} into database.", Color.LightSkyBlue);
            }
        }

        private static void ScrapeCarInfo(AppSettings settings)
        {
            var nonScrapedUrls = _dbHelper.GetNonScrapedUrls();

            Colorful.Console.WriteLine($"Downloading non-scraped car data for {nonScrapedUrls.Count} remaining urls.", Color.Orange);

            var interval = TimeSpan.FromMilliseconds(settings.RequestIntervalMs);

            int progress = 0;
            // we use batching to avoid losing progress
            foreach (var batch in nonScrapedUrls.Batch(50))
            {
                var cars = new List<Car>();

                Colorful.Console.WriteLine($"Start scraping batch of cars...", Color.GreenYellow);

                foreach (var url in batch)
                {
                    var scraper = new CarScraper(url, interval);
                    var car = scraper.ScrapeCarInformation();
                    if (car != null)
                    {
                        cars.Add(car);
                    }
                    Console.Title = $"Scraped {++progress} / {nonScrapedUrls.Count} car urls ({(double)progress / nonScrapedUrls.Count:P2}). Remaining time:{TimeSpan.FromMilliseconds((nonScrapedUrls.Count - progress)*150).ToLongString()}.";
                }

                var newCarIds = _dbHelper.InsertCars(cars);
                Colorful.Console.WriteLine($"Inserted {cars.Count} cars from batch into database...", Color.SkyBlue);
            }
        }

        private static void DownloadCarImages(AppSettings settings)
        {
            var cars = _dbHelper.GetCarsWithoutDownloadedImage();
            var interval = TimeSpan.FromMilliseconds(settings.RequestIntervalMs);

            int progress = 0;
            foreach (var car in cars)
            {
                var scraper = new ImageScraper(car.PictureUrl, interval);
                var path = scraper.DownloadImage(settings.ImagesDirectory);
                
                _dbHelper.UpdateCarImagePath(car, path);

                Console.Title = $"Scraped {++progress} / {cars.Count} car images ({(double)progress / cars.Count:P2}). Remaining time:{TimeSpan.FromMilliseconds((cars.Count - progress) * 150).ToLongString()}.";
            }
        }
    }
}
