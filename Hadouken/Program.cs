using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hadouken.Bots;
using Hadouken.Configuration;
using Hadouken.Database;
using Hadouken.Database.Repositories;
using Hadouken.Services;
using Hadouken.Commands;

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

            using (var context = serviceProvider.GetService<HadoukenContext>())
            {
                context.Database.Migrate();
            }

            serviceProvider
                .GetRequiredService<HadoukenBot>()
                .Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddOptions();

            services.AddDbContext<HadoukenContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("BotContext"));
            });

            services.Configure<BotConfiguration>(Configuration.GetSection("Bot"));

            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IQuoteRepository, QuoteRepository>();

            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IQuoteService, QuoteService>();

            services.AddScoped<IHelpCommand, HelpCommand>();
            services.AddScoped<IQuoteCommand, QuoteCommand>();
            services.AddScoped<ITalkCommand, TalkCommand>();
            services.AddScoped<IYellBarfCommand, YellBarfCommand>();

            services.AddTransient<HadoukenBot>();

            return services;
        }
    }
}