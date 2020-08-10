using System;
using System.Linq;

using Hadouken.Bots;
using Hadouken.Database;
using Hadouken.Services;

namespace Hadouken.Commands
{
    public class QuoteCommand : ICommand
    {
        public string Trigger => "!quote";

        private readonly IQuoteService _quoteService;

        public QuoteCommand(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        public void Action(IBot bot, string channel, string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                var quote = _quoteService.GetRandomQuote();

                if (quote == null)
                {
                    bot.SendMessage("There are no quotes to display", channel);
                }
                else
                {
                    bot.SendMessage($"{quote.Nick}: {quote.Content}", channel);
                }
            }
            else
            {
                var split = args.Split(" ");

                if (split.Length < 3)
                {
                    bot.SendMessage("Usage: !quote | !quote add <nick> <content>", channel);
                }
                else if (split[0].ToLower().Equals("add"))
                {
                    var quote = _quoteService.AddQuote(new Quote
                    {
                        Nick = split[1],
                        Content = string.Join(" ", split.Skip(2)),
                        Created = DateTime.UtcNow
                    });

                    if (quote != null)
                    {
                        bot.SendMessage($"Added quote #{quote.Id}", channel);
                    }
                    else
                    {
                        bot.SendMessage("There was a problem saving that quote, blame rhaydeo.", channel);
                    }
                }
            }
        }
    }
}