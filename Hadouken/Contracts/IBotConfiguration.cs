using Hadouken.Configuration;

namespace Hadouken.Contracts
{
    public interface IBotConfiguration
    {
        Identity Identity { get; set; }
        IrcServer IrcServer { get; set; }
        Flags Flags { get; set; }
    }
}
