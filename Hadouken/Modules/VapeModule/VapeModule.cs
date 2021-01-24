using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Discord.Commands;

namespace Hadouken.Modules.Quotes
{
    public class VapeModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<VapeModule> _logger;

        public VapeModule(
            ILogger<VapeModule> logger)
        {
            _logger = logger;
        }

        [Command("vape30")]
        [Summary("It's VAPE:30 boys")]
        public Task VapeAsync()
        {
            return Task.CompletedTask;
        }
    }
}