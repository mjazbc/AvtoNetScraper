using System.Collections.Generic;
using AvtoNetScraper.Database;
using AvtoNetScraper.Settings;
using Telegram.Bot;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using System;
using System.Linq;

namespace AvtoNetScraper.Notifications
{
    public class TelegramNotifier
    {
        private readonly Telegram.Bot.TelegramBotClient _bot;
        private readonly long _chatId;

        public TelegramNotifier(AppSettings settings)
        {
            _chatId = long.Parse(settings.TelegramChatId);
            _bot = new TelegramBotClient(settings.TelegramToken);
        }

        public async Task SendNotificationsForCarsAsync(IList<Car> cars, IList<Url> urls)
        {   
            foreach(var car in cars)
            {
                var text = FormatMessage(car, urls.FirstOrDefault(x => x.Id == car.UrlId).Address);
                var t = await _bot.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(_chatId), text, ParseMode.Html);
            }
            
        }

        private string FormatMessage(Car c, string url){
            
            return $"<b><a href=\"{url}\">{c.Name}</a></b>\n{c.Details}" +
                $"\nCena: {(int?)c.Price} €\nPrevoženi km: {c.MileageInKm}\nPrva registracija: {c.FirstRegistration?.ToString("MM yyyy")}\n\n";
        }
    }
}