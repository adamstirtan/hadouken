using System;
using System.IO;

using ChatSharp;

using Newtonsoft.Json;

using Hadouken.Bots;
using Hadouken.Configuration;

namespace Hadouken
{
    internal class Program
    {
        private static void Main()
        {
            var configuration = JsonConvert.DeserializeObject<BotConfiguration>(
                File.ReadAllText(Path.Combine(GetConfigurationDirectory(), "configuration.json")));

            var client = new IrcClient(
                $"{configuration.IrcServer.ServerName}:{configuration.IrcServer.ServerPort}",
                new IrcUser(
                    configuration.Identity.Nick,
                    configuration.Identity.UserName,
                    configuration.Identity.Password,
                    configuration.Identity.RealName),
                configuration.IrcServer.UseSsl);

            var bot = new HadoukenBot(client, configuration);

            bot.RunAndBlock();
        }

		// TODO this should probably return null and throw FileNotFoundEx
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
