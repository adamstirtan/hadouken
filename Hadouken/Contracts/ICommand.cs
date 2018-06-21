using ChatSharp;

namespace Hadouken.Contracts
{
    public interface ICommand
    {
        string Trigger { get; }

        void Action(IrcClient client, string channel, string args);
    }
}
