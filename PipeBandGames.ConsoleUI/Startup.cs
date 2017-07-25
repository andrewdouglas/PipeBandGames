using Microsoft.Extensions.Configuration;
using System;

namespace PipeBandGames.ConsoleUI
{
    public class Startup
    {
        private IConfigurationRoot configuration { get; }

        public Startup()
        {
            // TODO: Figure out configuration files in .NET Core 1.1
            // See: https://gist.github.com/CuddleBunny/9805d2042b0dfa620630fa38724895d0
            // and https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.configuration = builder.Build();

            ////Console.WriteLine($"Hi {configuration["foo"]}");
        }
    }
}
