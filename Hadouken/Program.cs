using System;
using System.Linq;
using System.IO;
using System.Threading;

using ChatSharp;

using Newtonsoft.Json;

using Hadouken.Bots;
using Hadouken.Configuration;
using Hadouken.Database;

namespace Hadouken
{
    internal class Program
    {
        private static ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            var configurationFile = "configuration.json";

            if (args.Any())
            {
                configurationFile = args.First();
            }

            Console.WriteLine($"Configuration: {configurationFile}");

            var configuration = JsonConvert.DeserializeObject<BotConfiguration>(
                File.ReadAllText(Path.Combine(GetConfigurationDirectory(), configurationFile)));

            var client = new IrcClient(
                $"{configuration.IrcServer.ServerName}:{configuration.IrcServer.ServerPort}", new IrcUser(
                    configuration.Identity.Nick,
                    configuration.Identity.UserName,
                    configuration.Identity.Password,
                    configuration.Identity.RealName),
                    configuration.IrcServer.UseSsl);

            Console.CancelKeyPress += (sender, args) =>
            {
                QuitEvent.Set();
                args.Cancel = true;
            };

            using (var db = new HadoukenContext())
            {
                db.Database.EnsureCreated();
            }

            new HadoukenBot(client, configuration)
                .Run();

            QuitEvent.WaitOne();
        }

        private static string GetConfigurationDirectory()
        {
            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            if (baseDirectory.Parent?.Parent?.Parent != null)
            {
                return baseDirectory.Parent.Parent.Parent.FullName;
            }

            throw new DirectoryNotFoundException();
        }
    }
}
