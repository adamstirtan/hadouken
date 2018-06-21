using ChatSharp;

namespace Hadouken
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var botUser = new IrcUser("hadouken", "hadouken");

            var client = new IrcClient("irc.freenode.net", botUser);

            client.ConnectAsync();

            while (true) { }
        }
    }
}
