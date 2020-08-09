using Microsoft.Extensions.Options;

using Hadouken.Commands;
using Hadouken.Configuration;

namespace Hadouken.Bots
{
    public class HadoukenBot : BaseBot
    {
        public HadoukenBot(IOptions<BotConfiguration> options)
            : base(options)
        {
            Commands.Add(new EchoCommand());
            Commands.Add(new EightBallCommand());
            Commands.Add(new YellBarfCommand());
            Commands.Add(new QuoteCommand());
            Commands.Add(new AolSayCommand());
            Commands.Add(new GiphyCommand());
            Commands.Add(new TalkCommand());
            Commands.Add(new AolTalkCommand());
            Commands.Add(new UrbanDictionaryCommand());
        }
    }
}