using ChatSharp;

using Hadouken.Contracts;

namespace Hadouken.Commands
{
    public sealed class EchoCommand : ICommand
    {
        public string Trigger => "!echo";

        public void Action(IrcClient client, string channel, string args)
        {
            client.SendMessage(args, channel);
        }
    }
}
