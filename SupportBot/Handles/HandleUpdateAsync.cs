using Telegram.Bot;
using Telegram.Bot.Types;

namespace SupportBot;


internal partial class Program
{
    private static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
    {
        var message = update.Message;

        if (message != null)
        {
            (long chatId, int? threadId) = await DefinitionIdAsync(message, client, token);

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

        return;
    }
}
