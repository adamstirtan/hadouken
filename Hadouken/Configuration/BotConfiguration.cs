using Hadouken.Contracts;

namespace Hadouken.Configuration
{
    public sealed class BotConfiguration : IBotConfiguration
    {
        public Identity Identity { get; set; }
        public IrcServer IrcServer { get; set; }
        public Flags Flags { get; set; }
    }
}
