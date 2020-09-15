using System;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Hadouken.Bots;
using Hadouken.Commands;
using Hadouken.Configuration;
using Hadouken.Database;
using Hadouken.Database.Repositories;
using Hadouken.Services;

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

            var flags = Configuration
                .GetSection("Bot")
                .GetSection("Flags");

            if (flags.GetValue<bool>("InitializeDatabase"))
            {
                try
                {
                    using var scope = serviceProvider
                        .GetRequiredService<IServiceScopeFactory>()
                        .CreateScope();

                    scope.ServiceProvider
                        .GetRequiredService<HadoukenContext>()
                        .Database
                        .Migrate();
                }
                catch (SqlException)
                {
                    Console.WriteLine("Unable to connect to the database in appsettings.json");
                    Environment.Exit(-1);
                }
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
            services.AddScoped<IAolSayCommand, AolSayCommand>();
            services.AddScoped<IAolTalkCommand, AolTalkCommand>();
            services.AddScoped<IEightBallCommand, EightBallCommand>();
            services.AddScoped<IGiphyCommand, GiphyCommand>();
            services.AddScoped<IQuoteCommand, QuoteCommand>();
            services.AddScoped<ITalkCommand, TalkCommand>();
            services.AddScoped<IUrbanDictionaryCommand, UrbanDictionaryCommand>();
            services.AddScoped<IYellBarfCommand, YellBarfCommand>();

            services.AddTransient<HadoukenBot>();

            return services;
        }
    }
}