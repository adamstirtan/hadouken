using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Hadouken.Bots;
using Hadouken.Configuration;

namespace Hadouken.Database
{
    public class DiscordBot : IBot
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DiscordBot> _logger;
        private readonly BotConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public DiscordBot(
            IServiceProvider serviceProvider,
            IOptions<BotConfiguration> options,
            ILogger<DiscordBot> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = options.Value;
            _logger = logger;

            _client = new DiscordSocketClient();
            _commands = new CommandService();
        }

        public async Task StartAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);

            await _client.LoginAsync(TokenType.Bot, _configuration.Credentials.Token);
            await _client.StartAsync();
        }

        public async Task StopAsync()
        {
            if (_client != null)
            {
                await _client.StopAsync();
            }
        }

        private async Task HandleCommandAsync(SocketMessage arg)
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
        }
    }
}