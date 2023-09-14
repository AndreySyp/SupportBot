using SupportBot.Infrastructure;
using SupportBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SupportBot;


internal partial class Program
{
    private static readonly Startup settings = new();
    private static HashSet<Models.User> users = new();


    private static void Main(string[] args)
    {
        using (ApplicationContext db = new(settings.Settings.ConconnectionSQl))
        {
            users = db.Users.ToHashSet();
        }
        var botClient = new TelegramBotClient(settings.Settings.BotToken);

        botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync);

        Console.WriteLine("Started");
        Console.ReadLine();
    }
}
