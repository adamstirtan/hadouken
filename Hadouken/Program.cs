using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            try
            {
                using var scope = serviceProvider
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();

                scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>()
                    .Database
                    .Migrate();
            }
            catch (SqlException)
            {
                Console.WriteLine("Unable to connect to the database in appsettings.json");
                Environment.Exit(-1);
            }

            await serviceProvider
                .GetRequiredService<IBot>()
                .StartAsync();
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddOptions();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("HadoukenConnection"));
            });

            services.Configure<BotConfiguration>(Configuration.GetSection("Bot"));

            services.AddTransient<DiscordBot>();

            return services;
        }
    }
}