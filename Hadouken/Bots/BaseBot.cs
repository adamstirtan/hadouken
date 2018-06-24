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
            client.ChannelMessageRecieved += ChannelMessageReceived;
        }

        public IrcClient Client { get; }

        public IBotConfiguration Configuration { get; }

        public List<ICommand> Commands { get; set; }

        public void RunAndBlock()
        {
            Client.ConnectAsync();

            Console.WriteLine("Hadouken bot started");
            Console.WriteLine("--------------------\n");
            Console.WriteLine("Press 'q' to quit");

            while (true)
            {
                var input = Console.ReadKey();

                if (input.KeyChar == 'q')
                {
                    break;
                }
            }
        }

        public void ConnectionComplete(object sender, EventArgs e)
        {
            foreach (var channel in Configuration.IrcServer.AutoJoinChannels)
            {
                Client.JoinChannel(channel);
            }
        }

        public void ChannelMessageReceived(object sender, PrivateMessageEventArgs e)
        {
            var content = e.PrivateMessage.Message;
            var nick = e.PrivateMessage.User.Nick;

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