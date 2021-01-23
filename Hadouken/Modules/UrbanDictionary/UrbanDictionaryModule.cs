using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Discord.Commands;
using Discord;

namespace Hadouken.Modules.UrbanDictionary
{
    public class UrbanDictionaryModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<UrbanDictionaryModule> _logger;
        private readonly HttpClient _httpClient;

        public UrbanDictionaryModule(ILogger<UrbanDictionaryModule> logger)
        {
            _logger = logger;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.urbandictionary.com/v0/"),
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        [Command("ud")]
        [Summary("Define a term using urbandictionary.com. Usage: !ud <term>")]
        public async Task DefineAsync([Remainder] string term)
        {
            _logger.LogInformation($"Attempting urban dictionary search: {term}");

            if (string.IsNullOrEmpty(term))
            {
                await ReplyAsync($"Usage: !ud <term>");
                return;
            }

            try
            {
                var response = await _httpClient.GetFromJsonAsync<UrbanDictionaryResponseDto>($"define?term={term}");

                if (response.List.Any())
                {
                    var definition = response.List.First();

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(54, 160, 244))
                        .WithTitle(definition.Word)
                        .WithUrl(definition.Permalink)
                        .WithDescription(definition.Definition)
                        .WithFooter(definition.Example, "https://freepps.top/images/uploads/soft/2016-12-23/urban-dictionary-material-logo-fit-wX-150.png");

                    await ReplyAsync(embed: builder.Build());
                }
                else
                {
                    await ReplyAsync($"There's no definition for \"{term}\" on urbandictionary.com");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return;
            }
        }
    }
}