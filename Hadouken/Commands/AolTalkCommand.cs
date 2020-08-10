﻿using System;

using Markov;

using Hadouken.Bots;
using Hadouken.Common;

namespace Hadouken.Commands
{
    public sealed class AolTalkCommand : IAolTalkCommand
    {
        public string Trigger => "!aoltalk";

        public void Action(IBot bot, string channel, string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                bot.SendMessage($"Usage: {Trigger}", channel);
                return;
            }

            var aolSay = new AolSay();
            var chain = new MarkovChain<string>(1);

            foreach (var response in aolSay.Responses)
            {
                chain.Add(response.Split(" "));
            }

            var result = string.Join(" ", chain.Chain(new Random(DateTime.UtcNow.Millisecond)));

            bot.SendMessage(result, channel);
        }
    }
}