using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Quartz;
using Quartz.Impl;

using Hadouken.Bots;
using Hadouken.Configuration;
using Hadouken.Database.Services;
using Hadouken.ObjectModel;

namespace Hadouken.Database
{
    public class DiscordBot : IBot
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DiscordBot> _logger;
        private readonly IMessageService _messageService;
        private readonly BotConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        private IScheduler _scheduler;

        public DiscordBot(
            IServiceProvider serviceProvider,
            IOptions<BotConfiguration> options,
            ILogger<DiscordBot> logger,
            IMessageService messageService)
        {
            _serviceProvider = serviceProvider;
            _configuration = options.Value;
            _logger = logger;
            _messageService = messageService;

            _client = new DiscordSocketClient();
            _commands = new CommandService();
        }

        public async Task RunAsync()
        {
            _client.MessageReceived += HandleMessageReceivedAsync;
            _client.MessageDeleted += HandleMessageDeletedAsync;
            _client.Disconnected += HandleDisconnectedAsync;

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);

            await ConnectAsync();
        }

        public async Task StopAsync()
        {
            if (_client is not null)
            {
                await _client.StopAsync();
            }
        }

        private async Task ConnectAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _configuration.Credentials.Token);
            await _client.StartAsync();
            await ScheduleTasksAsync();
        }

        private async Task ScheduleTasksAsync()
        {
            var factory = new StdSchedulerFactory();

            _scheduler = await factory.GetScheduler();
        }

        private async Task HandleMessageReceivedAsync(SocketMessage arg)
        {
            if (arg is not SocketUserMessage message)
            {
                return;
            }

            if (message.Author.IsBot)
            {
                return;
            }

            int position = 0;

            if (message.HasCharPrefix('!', ref position))
            {
                await _commands.ExecuteAsync(new SocketCommandContext(_client, message), position, null);
            }
            else
            {
                var entity = await _messageService.CreateAsync(new Message
                {
                    Content = message.Content,
                    UserName = message.Author.Username,
                    Timestamp = message.CreatedAt.UtcDateTime
                });

                _logger.LogInformation($"{entity.UserName}: {entity.Content}");
            }
        }

        private async Task HandleDisconnectedAsync(Exception arg)
        {
            _logger.LogError(arg.Message);

            if (_scheduler is not null && !_scheduler.IsShutdown)
            {
                await _scheduler.Shutdown();
            }

            await ConnectAsync();
        }

        private async Task HandleMessageDeletedAsync(Cacheable<IMessage, ulong> message, ISocketMessageChannel channel)
        {
            await channel.SendMessageAsync("I saw that.");
        }
    }
}