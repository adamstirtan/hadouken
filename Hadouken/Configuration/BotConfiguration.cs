using Hadouken.Contracts;

namespace Hadouken.Configuration
{
    public class BotConfiguration : IBotConfiguration
    {
        public BotIdentity Identity { get; set; }
        public IrcServer IrcServer { get; set; }
        public string[] AutoJoinChannels { get; set; }
    }
}
