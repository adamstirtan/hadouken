using System.Linq;

using Hadouken.Contracts;

namespace Hadouken.Commands
{
    public sealed class HelpCommand : ICommand
    {
        public string Trigger => "!help";

        public void Action(IBot bot, string channel, string args)
        {
            var commands = bot.Commands.Where(x => x.Trigger != "!help").Select(x => x.Trigger);

            bot.Client.SendMessage($"Available commands: {string.Join(", ", commands)}", channel);
        }
    }
}
