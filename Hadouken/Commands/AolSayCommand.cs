using System;

using Hadouken.Common;
using Hadouken.Contracts;

namespace Hadouken.Commands
{
    public class AolSayCommand : ICommand
    {
        private static readonly Random Random = new Random();

        public string Trigger => "!aolsay";

        public void Action(IBot bot, string channel, string args)
        {
            var aolSay = new AolSay();

            bot.Client.SendMessage(aolSay.Responses[Random.Next(aolSay.Responses.Length)], channel);
        }
    }
}
