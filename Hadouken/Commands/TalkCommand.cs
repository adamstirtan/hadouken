using System;
using System.Linq;

using Markov;

using Hadouken.Contracts;
using Hadouken.Database;

namespace Hadouken.Commands
{
    public sealed class TalkCommand : ICommand
    {
        public string Trigger => "!talk";

        public void Action(IBot bot, string channel, string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                bot.Client.SendMessage($"Usage: {Trigger} <nick>", channel);
            }
            else
            {
                var split = args.Split(" ");

                if (split.Length != 1)
                {
                    bot.Client.SendMessage($"Usage: {Trigger} <nick>", channel);
                    return;
                }

                string[] messages;

                using (var db = new HadoukenContext())
                {
                    messages = db.Messages
                        .Where(x => x.Nick == split[0])
                        .Select(x => x.Content)
                        .ToArray();
                }

                if (messages.Length < 5)
                {
                    bot.Client.SendMessage($"{split[0]} hasn't said enough to make them talk", channel);
                    return;
                }

                var chain = new MarkovChain<string>(1);

                foreach (var message in messages)
                {
                    var spaceSplit = message.Split(" ");

                    if (spaceSplit.Length > 5)
                    {
                        chain.Add(spaceSplit);
                    }
                }

                var result = string.Join(" ", chain.Chain(new Random(DateTime.UtcNow.Millisecond)));

                bot.Client.SendMessage($"{split[0]}: {result}", channel);
            }
        }
    }
}
