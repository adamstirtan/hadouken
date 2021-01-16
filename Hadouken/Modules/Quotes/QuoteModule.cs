using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Discord.Commands;

using Hadouken.Database.Services;

namespace Hadouken.Modules.Quotes
{
    public class QuoteModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<QuoteModule> _logger;
        private readonly IQuoteService _quoteService;

        public QuoteModule(
            ILogger<QuoteModule> logger,
            IQuoteService quoteService)
        {
            _logger = logger;
            _quoteService = quoteService;
        }

        [Command("addquote")]
        [Summary("Adds a quote to the database")]
        public Task AddQuoteAsync(string userName, [Remainder] string content)
        {
            _logger.LogInformation($"Adding quote for {userName}: {content}");

            return Task.CompletedTask;
        }
    }
}