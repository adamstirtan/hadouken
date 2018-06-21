﻿using ChatSharp;

namespace Hadouken.Contracts
{
    public interface ICommand
    {
        string Trigger { get; }

        void Action(IBot bot, string channel, string args);
    }
}
