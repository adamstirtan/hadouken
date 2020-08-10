using System.Collections.Generic;

using Hadouken.Commands;
using Hadouken.Configuration;

namespace Hadouken.Bots
{
    public interface IBot
    {
        BotConfiguration Configuration { get; }

        List<ICommand> Commands { get; set; }

        void Run();

        void SendMessage(string message, string channel);
    }
}