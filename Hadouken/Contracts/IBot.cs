using System;
using System.Collections.Generic;

using ChatSharp;
using ChatSharp.Events;

namespace Hadouken.Contracts
{
    public interface IBot
    {
        IrcClient Client { get; }

        IBotConfiguration Configuration { get; }

        List<ICommand> Commands { get; set; }

        void RunAndBlock();

        void ConnectionComplete(object sender, EventArgs e);

        void ChannelMessageReceived(object sender, PrivateMessageEventArgs e);
    }
}
