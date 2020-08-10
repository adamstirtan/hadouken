using Microsoft.Extensions.Options;

using Hadouken.Commands;
using Hadouken.Configuration;
using Hadouken.Services;

namespace Hadouken.Bots
{
    public class HadoukenBot : BaseBot
    {
        private static IMessageService messageRepository;

        public HadoukenBot(
            IOptions<BotConfiguration> options,
            IMessageService messageService)
            : base(options, messageService)
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