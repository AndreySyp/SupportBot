using SupportBot.Handles;
using SupportBot.Infrastructure;
using Telegram.Bot;

namespace SupportBot;

internal class Program
{
    private static void Main(string[] args)
    {
        Startup startup = new();
        var botClient = new TelegramBotClient(startup.Settings.BotToken);

        HandleTelegramBot handleTelegramBot = new(startup.Settings);

        botClient.StartReceiving(handleTelegramBot.HandleUpdateAsync, HandleTelegramBot.HandleErrorAsync);

        Console.WriteLine("Started");
        Console.ReadLine();
    }
}
