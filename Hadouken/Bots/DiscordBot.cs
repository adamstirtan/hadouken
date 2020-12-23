using System;
using System.Reflection;
using System.Threading.Tasks;

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
        private readonly BotConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public DiscordBot(
            IServiceProvider serviceProvider,
            IOptions<BotConfiguration> options,
            DiscordSocketClient client,
            CommandService commands)
        {
            _configuration = options.Value;

            _client = client;
            _commands = commands;
        }

        public async Task StartAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), null);

            await _client.LoginAsync(TokenType.Bot, _configuration.Credentials.Token);
            await _client.StartAsync();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
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