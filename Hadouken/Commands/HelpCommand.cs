using System.Linq;

using Hadouken.Bots;

namespace Hadouken.Commands
{
    public sealed class HelpCommand : ICommand
    {
        public string Trigger => "!help";

        public void Action(IBot bot, string channel, string args)
        {
            var commands = bot.Commands
                .Where(x => x.Trigger != "!help")
                .OrderBy(x => x.Trigger)
                .Select(x => x.Trigger);

            bot.SendMessage($"Commands: {string.Join(", ", commands)}", channel);
        }
    }
}