using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hadouken.Bots;
using Hadouken.Configuration;
using Hadouken.Database;
using Hadouken.Database.Repositories;

namespace Hadouken
{
    internal class Program
    {
        private static IConfiguration Configuration;

        private static void Main()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            var db = serviceProvider.GetService<HadoukenContext>();
            db.Database.EnsureCreated();

            IBot bot = serviceProvider.GetService<HadoukenBot>();
            bot.Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddOptions();

            services.AddDbContext<HadoukenContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("BotContext"));
            });

            services.Configure<BotConfiguration>(Configuration.GetSection("Bot"));

            services.AddTransient<IMessageRepository, MessageRepository>();

            services.AddTransient<HadoukenBot>();

            return services;
        }
    }
}