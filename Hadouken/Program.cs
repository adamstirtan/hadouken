using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Hadouken.Bots;
using Hadouken.Configuration;
using Hadouken.Database;

namespace Hadouken
{
    internal class Program
    {
        private static IConfiguration Configuration;

        private static void Main(string[] args) =>
            MainAsync(args).GetAwaiter().GetResult();

        private static async Task MainAsync(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlite(Configuration.GetConnectionString("HadoukenConnection"));
                    });

                    services.Configure<BotConfiguration>(Configuration.GetSection("Bot"));

                    services.AddTransient<IBot, DiscordBot>();
                })
                .UseSerilog()
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

            await host.Services
                .GetRequiredService<IBot>()
                .StartAsync();
        }
    }
}