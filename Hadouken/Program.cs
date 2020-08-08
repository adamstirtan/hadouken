using System;
using System.Linq;
using System.IO;
using System.Threading;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ChatSharp;

using Newtonsoft.Json;

using Hadouken.Bots;
using Hadouken.Configuration;
using Hadouken.Database;
using Hadouken.Contracts;

namespace Hadouken
{
    internal class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            string configurationPath = null;
            string configurationFile = "configuration.json";

            if (args.Any())
            {
                configurationFile = args.First();
            }

            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            if (baseDirectory.Parent?.Parent?.Parent == null)
            {
                throw new DirectoryNotFoundException();
            }

            configurationPath = baseDirectory.Parent?.Parent?.Parent.FullName;

            var configuration = JsonConvert.DeserializeObject<BotConfiguration>(
                File.ReadAllText(Path.Combine(configurationPath, configurationFile)));

            var client = new IrcClient(
                $"{configuration.IrcServer.ServerName}:{configuration.IrcServer.ServerPort}", new IrcUser(
                    configuration.Identity.Nick,
                    configuration.Identity.UserName,
                    configuration.Identity.Password,
                    configuration.Identity.RealName),
                    configuration.IrcServer.UseSsl);

            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = true;

                QuitEvent.Set();
            };

            //using (var db = new HadoukenContext())
            //{
            //    db.Database.EnsureCreated();
            //}

            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            IBot bot = serviceProvider.GetService<HadoukenBot>();
            bot.Run();

            //QuitEvent.WaitOne();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<HadoukenContext>(options =>
            {
                // Issue: https://github.com/adamstirtan/hadouken/issues/1
                options.UseSqlite("bot.db");
            });

            services.AddTransient<HadoukenBot>();

            return services;
        }
    }
}
