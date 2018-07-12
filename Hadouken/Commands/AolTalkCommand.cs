using System;

using Markov;

using Hadouken.Common;
using Hadouken.Contracts;

namespace Hadouken.Commands
{
    public sealed class AolTalkCommand : ICommand
    {
        public string Trigger => "!aoltalk";

        public void Action(IBot bot, string channel, string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                bot.Client.SendMessage($"Usage: {Trigger}", channel);
                return;
            }

            var aolSay = new AolSay();
            var chain = new MarkovChain<string>(1);

            foreach (var response in aolSay.Responses)
            {
                chain.Add(response.Split(" "));
            }

            var result = string.Join(" ", chain.Chain(new Random(DateTime.UtcNow.Millisecond)));

            bot.Client.SendMessage(result, channel);
        }
    }
}
