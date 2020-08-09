using Hadouken.Bots;

namespace Hadouken.Commands
{
    public interface ICommand
    {
        string Trigger { get; }

        void Action(IBot bot, string channel, string args);
    }
}