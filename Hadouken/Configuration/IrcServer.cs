namespace Hadouken.Configuration
{
    public sealed class IrcServer
    {
        public string ServerName { get; set; }

        public int ServerPort { get; set; }

        public bool UseSsl { get; set; }

        public string[] AutoJoinChannels { get; set; }
    }
}