using Microsoft.Extensions.Configuration;
using SupportBot.Models;

namespace SupportBot.Infrastructure;


public class Startup
{
    public Startup()
    {
        var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = builder.Build();

        Settings = config.Get<Settings>();
    }

    public Settings Settings { get; private set; }
}
