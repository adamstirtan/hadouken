using System;
using System.Collections.Generic;
using System.Linq;

using ChatSharp;
using ChatSharp.Events;

using Hadouken.Commands;
using Hadouken.Contracts;
using Hadouken.Database;

namespace Hadouken.Bots
{
    public abstract class BaseBot : IBot
    {
        protected BaseBot(IrcClient client, IBotConfiguration configuration)
        {
            Client = client;
            Configuration = configuration;
            Commands = new List<ICommand>
            {
                new HelpCommand()
            };

            client.ConnectionComplete += ConnectionComplete;
            client.UserKicked += UserKicked;
            client.ChannelMessageRecieved += ChannelMessageReceived;
        }

        public IrcClient Client { get; }

        public IBotConfiguration Configuration { get; }

        public List<ICommand> Commands { get; set; }

        public void RunAndBlock()
        {
            Client.ConnectAsync();

            while (true)
            {
                Console.Read();
            }
        }

        public void ConnectionComplete(object sender, EventArgs e)
        {
            foreach (var channel in Configuration.IrcServer.AutoJoinChannels)
            {
                Client.JoinChannel(channel);
            }

            Client.SendMessage($"identify {Configuration.Identity.Password}", "nickserv");
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

        public void UserPartedChannel(object sender, ChannelUserEventArgs e)
        {
            if (e.User.Nick == Configuration.Identity.Nick)
            {
                Client.JoinChannel(e.Channel.Name);
            }
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
                var command = Commands
                    .FirstOrDefault(x => string.Equals(x.Trigger, split[0], StringComparison.InvariantCultureIgnoreCase));

                command?.Action(this, e.PrivateMessage.Source, string.Join(" ", split.Skip(1)));
            }
            else
            {
                using (var db = new HadoukenContext())
                {
                    db.Messages.Add(new Message
                    {
                        Content = content,
                        Nick = nick,
                        Created = DateTime.UtcNow
                    });

                    db.SaveChanges();
                }
            }
        }
    }
}
