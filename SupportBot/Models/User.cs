namespace SupportBot.Models;


public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long ChatId { get; set; }
    public int ThreadId { get; set; }

    public User(string name, long chatId, int threadId)
    {
        Name = name;
        ChatId = chatId;
        ThreadId = threadId;
    }
}
