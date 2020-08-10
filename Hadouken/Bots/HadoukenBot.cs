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
            IAolSayCommand aolSayCommand,
            IAolTalkCommand aolTalkCommand,
            IEightBallCommand eightBallCommand,
            IGiphyCommand giphyCommand,
            ITalkCommand talkCommand,
            IUrbanDictionaryCommand urbanDictionaryCommand,
            IYellBarfCommand yellBarfCommand,
            IQuoteCommand quoteCommand)
            : base(options, messageService)
        {
            Commands.Add(aolSayCommand);
            Commands.Add(aolTalkCommand);
            Commands.Add(eightBallCommand);
            Commands.Add(giphyCommand);
            Commands.Add(quoteCommand);
            Commands.Add(talkCommand);
            Commands.Add(urbanDictionaryCommand);
            Commands.Add(yellBarfCommand);
        }
    }
}