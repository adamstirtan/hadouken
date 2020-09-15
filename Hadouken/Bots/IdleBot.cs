using Microsoft.Extensions.Options;

using Hadouken.Configuration;
using Hadouken.Services;

namespace Hadouken.Bots
{
    public class IdleBot : BaseBot
    {
        public IdleBot(
            IOptions<BotConfiguration> options,
            IMessageService messageService)
            : base(options, messageService)
        { }
    }
}