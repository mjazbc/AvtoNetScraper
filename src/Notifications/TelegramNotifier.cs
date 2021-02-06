using System.Collections.Generic;
using AvtoNetScraper.Database;
using AvtoNetScraper.Settings;
using Telegram.Bot;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace AvtoNetScraper.Notifications
{
    public class TelegramNotifier
    {
        private readonly Telegram.Bot.TelegramBotClient _bot;
        private readonly string _chatId;

        public TelegramNotifier(AppSettings settings)
        {
            _chatId = settings.TelegramChatId;
            _bot = new TelegramBotClient(settings.TelegramToken);
        }

        public async Task SendNotificationsForCarsAsync(IList<Car> cars)
        {   
            foreach(var car in cars)
            {
                var text = FormatMessage(car);
                var t = await _bot.SendTextMessageAsync(_chatId, text, parseMode: ParseMode.Html);
            }
            
        }

        private string FormatMessage(Car c){
            
            return $"<a href=\"{c.PictureUrl}\">&#8205;</a>";
        }
    }
}