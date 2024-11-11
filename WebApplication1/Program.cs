using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Чтение токена из конфигурации
            var botToken = builder.Configuration["TelegramBotToken"];

            // Добавление TelegramBotClient
            builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient("7432101128:AAFHcX5aM7CJBXGToKOE0FxfSif0zIvZt3o"));

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
