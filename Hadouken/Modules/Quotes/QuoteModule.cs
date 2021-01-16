using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Discord.Commands;

using Hadouken.Database.Services;
using Hadouken.ObjectModel;

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

        [Command("quote")]
        [Summary("Says a random quote")]
        public async Task RandomQuoteAsync()
        {
            _logger.LogInformation("Saying a random quote");

            var rng = new Random(DateTime.UtcNow.Millisecond);

            var quotes = _quoteService
                .All()
                .ToArray();

            if (quotes.Any())
            {
                var quote = quotes[rng.Next(0, quotes.Length)];

                await ReplyAsync($"@{quote.UserName}: {quote.Content} (#{quote.Id})");
            }
            else
            {
                await ReplyAsync("There are no quotes in the database yet. Usage: !addquote <UserName> <Content>");
            }
        }

        [Command("quote")]
        [Summary("Says a specific quote")]
        public async Task GetQuoteAsync(long id)
        {
            _logger.LogInformation($"Saying quote #{id}");

            var quote = await _quoteService.GetByIdAsync(id);

            if (quote is null)
            {
                await ReplyAsync($"Woah girl, that's a 404 because that quote doesn't exist");
            }
            else
            {
                await ReplyAsync($"@{quote.UserName}: {quote.Content} (#{quote.Id})");
            }
        }

        [Command("addquote")]
        [Summary("Adds a quote to the database")]
        public async Task AddQuoteAsync(string userName, [Remainder] string content)
        {
            _logger.LogInformation($"Adding quote for {userName}: {content}");

            try
            {
                var quote = await _quoteService.CreateAsync(new Quote
                {
                    UserName = userName,
                    Content = content
                });

                await ReplyAsync($"Cha-ching! That gem of a quote will haunt them forever. (#{quote.Id})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        [Command("delquote")]
        [Summary("Remove a specific quote")]
        public async Task RemoveQuoteAsync(long id)
        {
            _logger.LogInformation($"Attempting to remove quote {id}");

            try
            {
                var quote = await _quoteService.GetByIdAsync(id);

                if (quote is null)
                {
                    await ReplyAsync($"Woah girl, that's a 404 because that quote doesn't exist");
                }
                else
                {
                    if (await _quoteService.DeleteAsync(id))
                    {
                        await ReplyAsync("He's dead, Jim.");
                    }
                    else
                    {
                        await ReplyAsync("Something went wrong, better talk to @rhaydeo");
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
    }
}