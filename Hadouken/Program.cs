using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Hadouken.Bots;
using Hadouken.Configuration;
using Hadouken.Database;
using Hadouken.Database.Services;

namespace Hadouken
{
    internal class Program
    {
        private static IConfiguration Configuration;

        private static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlite(Configuration.GetConnectionString("HadoukenConnection"));
                    });

                    services.Configure<BotConfiguration>(Configuration.GetSection("Bot"));

                    services.AddScoped<IMessageService, MessageService>();
                    services.AddScoped<IQuoteService, QuoteService>();

                    services.AddTransient<IBot, DiscordBot>();
                })
                .Build();

            try
            {
                Log.Information("Migrating database");

                using var scope = host.Services
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();

                scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>()
                    .Database
                    .Migrate();
            }
            catch (SqlException)
            {
                Log.Fatal("Unable to connect to the database in appsettings.json");
                Environment.Exit(-1);
            }

            var exitEvent = new ManualResetEvent(false);

            AssemblyLoadContext.Default.Unloading += (context) =>
            {
                Log.Information("Bot was stopped");

                exitEvent.Set();
            };

            Console.CancelKeyPress += (sender, e) =>
            {
                Log.Information("Stopping bot");

                e.Cancel = true;
                exitEvent.Set();
            };

            using (var scope = host.Services
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var bot = scope.ServiceProvider.GetRequiredService<IBot>();

                await bot.RunAsync();
                exitEvent.WaitOne();
                await bot.StopAsync();
            }

            Environment.Exit(0);
        }
    }
}