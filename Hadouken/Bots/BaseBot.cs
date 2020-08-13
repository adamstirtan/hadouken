using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Extensions.Options;

using ChatSharp;
using ChatSharp.Events;

using Hadouken.Commands;
using Hadouken.Configuration;
using Hadouken.ObjectModel;
using Hadouken.Services;

namespace Hadouken.Bots
{
    public abstract class BaseBot : IBot
    {
        private readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private readonly IMessageService _messageService;
        private readonly IrcClient _client;

        protected BaseBot(
            IOptions<BotConfiguration> options,
            IMessageService messageService)
        {
            Configuration = options.Value;

            _messageService = messageService;

            _client = new IrcClient(
                $"{Configuration.IrcServer.ServerName}:{Configuration.IrcServer.ServerPort}", new IrcUser(
                    Configuration.Identity.Nick,
                    Configuration.Identity.UserName,
                    Configuration.Identity.Password,
                    Configuration.Identity.RealName),
                Configuration.IrcServer.UseSsl);

            Commands = new List<ICommand>
            {
                new HelpCommand()
            };

            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = true;

                QuitEvent.Set();
            };

            _client.ConnectionComplete += ConnectionComplete;
            _client.ChannelMessageRecieved += ChannelMessageReceived;
            _client.UserKicked += UserKicked;
            _client.UserJoinedChannel += UserJoinedChannel;
            _client.UserPartedChannel += UserPartedChannel;
        }

        public IrcClient Client { get; }

        public BotConfiguration Configuration { get; }

        public List<ICommand> Commands { get; set; }

        public void Run()
        {
            _client.ConnectAsync();

            QuitEvent.WaitOne();
        }

        public void SendMessage(string message, string channel)
        {
            _client.SendMessage(message, channel);
        }

        public void ConnectionComplete(object sender, EventArgs e)
        {
            foreach (var channel in Configuration.IrcServer.AutoJoinChannels)
            {
                if (channel.Password == null)
                {
                    _client.JoinChannel($"{channel.Name}");
                }
                else
                {
                    _client.JoinChannel($"{channel.Name} {channel.Password}");
                }
            }

            _client.SendMessage($"identify {Configuration.Identity.Password}", "nickserv");
        }

        public void ChannelMessageReceived(object sender, PrivateMessageEventArgs e)
        {
            var message = e.PrivateMessage.Message;
            var nick = e.PrivateMessage.User.Nick;

            if (Configuration.Flags.ConsoleLogging)
            {
                Console.WriteLine($"<{nick}>: {message}");
            }

            if (message.StartsWith("!"))
            {
                var split = message.Split(" ");
                var command = Commands.FirstOrDefault(x =>
                    string.Equals(x.Trigger, split[0], StringComparison.InvariantCultureIgnoreCase));

                command?.Action(this, e.PrivateMessage.Source, string.Join(" ", split.Skip(1)));
            }
            else
            {
                _messageService.AddMessage(new Message
                {
                    Content = message,
                    Nick = nick,
                    Created = DateTime.UtcNow
                });
            }
        }

        private void UserKicked(object sender, KickEventArgs e)
        {
            if (Configuration.Flags.RejoinOnKick)
            {
                if (e.Kicked.Nick == Configuration.Identity.Nick)
                {
                    _client.JoinChannel(e.Channel.Name);
                }
            }
        }

        private void UserJoinedChannel(object sender, ChannelUserEventArgs e)
        { }

        public void UserPartedChannel(object sender, ChannelUserEventArgs e)
        {
            if (e.User.Nick == Configuration.Identity.Nick)
            {
                _client.JoinChannel(e.Channel.Name);
            }
        }
    }
}