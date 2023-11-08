using SupportBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SupportBot;

public class ProgramHelpers
{
    public static async Task<bool> AddUser(ITelegramBotClient client, Settings settings, HashSet<Models.User> users, Message? message, CancellationToken token)
    {
        string username = message.From.Username;
        if (!users.All(x => x.Name != username)) return false;

        ForumTopic forumTopic = await client.CreateForumTopicAsync(settings.GroupId, username, cancellationToken: token);

        Models.User user = new(username, message.Chat.Id, forumTopic.MessageThreadId);
        users.Add(user);

        using ApplicationContext db = new(settings.ConconnectionSQl);
        db.Users.AddRange(user);
        db.SaveChanges();

        return true;
    }

    public static async Task<Tuple<long, int?>> DefinitionIdAsync(ITelegramBotClient client, Settings settings, HashSet<Models.User> users, Message? message, CancellationToken token)
    {
        long chatId;
        int? threadId;

        if (message.MessageThreadId == null)
        {
            await AddUser(client, settings, users, message, token);

            chatId = settings.GroupId;
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
