using Hadouken.Configuration;

namespace Hadouken.Contracts
{
    public interface IBotConfiguration
    {
        BotIdentity Identity { get; set; }
        IrcServer IrcServer { get; set; }
        string[] AutoJoinChannels { get; set; }
    }
}
