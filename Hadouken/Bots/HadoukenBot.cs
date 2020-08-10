using Microsoft.Extensions.Options;

using Hadouken.Commands;
using Hadouken.Configuration;
using Hadouken.Services;

namespace Hadouken.Bots
{
    public class HadoukenBot : BaseBot
    {
        public HadoukenBot(
            IOptions<BotConfiguration> options,
            IMessageService messageService,
            ITalkCommand talkCommand,
            IYellBarfCommand yellBarfCommand,
            IQuoteCommand quoteCommand)
            : base(options, messageService)
        {
            Commands.Add(new EchoCommand());
            Commands.Add(new EightBallCommand());
            Commands.Add(yellBarfCommand);
            Commands.Add(quoteCommand);
            Commands.Add(new AolSayCommand());
            Commands.Add(new GiphyCommand());
            Commands.Add(talkCommand);
            Commands.Add(new AolTalkCommand());
            Commands.Add(new UrbanDictionaryCommand());
        }
    }
}