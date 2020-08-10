using Hadouken.Bots;

namespace Hadouken.Commands
{
    public sealed class YellBarfCommand : IYellBarfCommand
    {
        public string Trigger => "!yellbarf";

        public void Action(IBot bot, string channel, string args)
        {
            bot.SendMessage($"{args.ToUpper()}!!", channel);
        }
    }
}