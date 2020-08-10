﻿using Hadouken.Bots;

namespace Hadouken.Commands
{
    public sealed class EchoCommand : ICommand
    {
        public string Trigger => "!echo";

        public void Action(IBot bot, string channel, string args)
        {
            bot.SendMessage(args, channel);
        }
    }
}