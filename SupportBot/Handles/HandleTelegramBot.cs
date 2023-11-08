using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using SupportBot.Models;

namespace SupportBot.Handles;

public class HandleTelegramBot
{
    private readonly Settings settings = new();
    private HashSet<Models.User> users = new();

    public HandleTelegramBot(Settings settings)
    {
        this.settings = settings;

        using ApplicationContext db = new(settings.ConconnectionSQl);
        users = db.Users.ToHashSet();
    }

    public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        var message = update.Message;
        if (message == null) return;

        (long chatId, int? threadId) = await ProgramHelpers.DefinitionIdAsync(client, settings, users, message, token);

        if (message.Text != null)
        {
            await client.SendTextMessageAsync(
                chatId,
                message.Text,
                threadId,
                cancellationToken: token);
        }
        if (message.Photo != null)
        {
            await client.SendPhotoAsync(
                chatId,
                new InputFileId(message.Photo![^1].FileId),
                threadId,
                caption: message.Caption,
                cancellationToken: token);
        }
        if (message.Document != null)
        {
            await client.SendDocumentAsync(
                chatId,
                new InputFileId(message.Document.FileId),
                threadId,
                caption: message.Caption,
                cancellationToken: token);
        }
    }
}
