using System;
using System.Collections.Generic;
using System.Linq;

using ChatSharp;
using ChatSharp.Events;

using Hadouken.Commands;
using Hadouken.Contracts;

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

            Console.ReadKey();
        }

        public void ConnectionComplete(object sender, EventArgs e)
        {
            foreach (var channel in Configuration.AutoJoinChannels)
            {
                Client.JoinChannel(channel);
            }
        }

        public void ChannelMessageReceived(object sender, PrivateMessageEventArgs e)
        {
            var content = e.PrivateMessage.Message;

            if (!content.StartsWith("!"))
            {
                return;
            }

            var split = content.Split(" ");
            var command = Commands
                .FirstOrDefault(x => string.Equals(x.Trigger, split[0], StringComparison.InvariantCultureIgnoreCase));

            command?.Action(this, e.PrivateMessage.Source, string.Join(" ", split.Skip(1)));
        }
    }
}
