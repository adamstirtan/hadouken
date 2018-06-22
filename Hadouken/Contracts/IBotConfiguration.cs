using Hadouken.Configuration;

namespace Hadouken.Contracts
{
    public interface IBotConfiguration
    {
        BotIdentity Identity { get; set; }
        IrcServer IrcServer { get; set; }
        Flags Flags { get; set; }
    }
}
