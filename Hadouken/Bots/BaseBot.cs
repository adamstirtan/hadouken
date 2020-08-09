using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Extensions.Options;

using ChatSharp;
using ChatSharp.Events;

using Hadouken.Commands;
using Hadouken.Configuration;
using Hadouken.Database.Repositories;
using Hadouken.Database;

namespace Hadouken.Bots
{
    public abstract class BaseBot : IBot
    {
        private readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private readonly IMessageRepository _messageRepository;

        protected BaseBot(
            IOptions<BotConfiguration> options,
            IMessageRepository messageRepository)
        {
            Configuration = options.Value;

            _messageRepository = messageRepository;

            Client = new IrcClient(
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

            Client.ConnectionComplete += ConnectionComplete;
            Client.ChannelMessageRecieved += ChannelMessageReceived;
            Client.UserKicked += UserKicked;
            Client.UserJoinedChannel += UserJoinedChannel;
            Client.UserPartedChannel += UserPartedChannel;
        }

        public IrcClient Client { get; }

        public BotConfiguration Configuration { get; }

        public List<ICommand> Commands { get; set; }

        public void Run()
        {
            Client.ConnectAsync();

            QuitEvent.WaitOne();
        }

        public void ConnectionComplete(object sender, EventArgs e)
        {
            foreach (var channel in Configuration.IrcServer.AutoJoinChannels)
            {
                Client.JoinChannel(channel);
            }

            Client.SendMessage($"identify {Configuration.Identity.Password}", "nickserv");
        }

        public void ChannelMessageReceived(object sender, PrivateMessageEventArgs e)
        {
            var content = e.PrivateMessage.Message;
            var nick = e.PrivateMessage.User.Nick;

            if (Configuration.Flags.ConsoleLogging)
            {
                Console.WriteLine($"<{nick}>: {content}");
            }

            if (content.StartsWith("!"))
            {
                var split = content.Split(" ");
                var command = Commands.FirstOrDefault(x =>
                    string.Equals(x.Trigger, split[0], StringComparison.InvariantCultureIgnoreCase));

                command?.Action(this, e.PrivateMessage.Source, string.Join(" ", split.Skip(1)));
            }
            else
            {
                _messageRepository.Create(new Message
                {
                    Content = content,
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
                    Client.JoinChannel(e.Channel.Name);
                }
            }
        }

        private void UserJoinedChannel(object sender, ChannelUserEventArgs e)
        { }

        public void UserPartedChannel(object sender, ChannelUserEventArgs e)
        {
            if (e.User.Nick == Configuration.Identity.Nick)
            {
                Client.JoinChannel(e.Channel.Name);
            }
        }
    }
}