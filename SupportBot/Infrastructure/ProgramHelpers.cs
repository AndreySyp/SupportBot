using SupportBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SupportBot;


internal partial class Program
{
    public static async Task<bool> AddUser(Message? message, ITelegramBotClient client, CancellationToken token)
    {
        string username = message.From.Username;

        if (users.All(x => x.Name != username))
        {
            ForumTopic forumTopic = await client.CreateForumTopicAsync(settings.Settings.GroupId, username, cancellationToken: token);
            var t = new Models.User(forumTopic.Name, message.Chat.Id, forumTopic.MessageThreadId);
            users.Add(t);
            using (ApplicationContext db = new(settings.Settings.ConconnectionSQl))
            {
                db.Users.AddRange(t);
                db.SaveChanges();
            }

            return true;
        }

        return false;
    }

    public static async Task<Tuple<long, int?>> DefinitionIdAsync(Message? message, ITelegramBotClient client, CancellationToken token)
    {
        long chatId;
        int? threadId;

        if (message.MessageThreadId == null)
        {
            await AddUser(message, client, token);

            chatId = settings.Settings.GroupId;
            threadId = users.First(x => x.Name == message.From.Username).ThreadId;
        }
        else
        {
            chatId = users.First(x => x.ThreadId == message.MessageThreadId).ChatId;
            threadId = null;
        }

        return new Tuple<long, int?>(chatId, threadId);
    }
}
