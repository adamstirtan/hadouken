using System.Collections.Generic;

using ChatSharp;

using Hadouken.Commands;
using Hadouken.Configuration;

namespace Hadouken.Bots
{
    public interface IBot
    {
        IrcClient Client { get; }

        BotConfiguration Configuration { get; }

        List<ICommand> Commands { get; set; }

        void Run();
    }
}