using System.Threading.Tasks;

using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
using GiphyDotNet.Model.Results;

using Hadouken.Bots;

namespace Hadouken.Commands
{
    public sealed class GiphyCommand : IGiphyCommand
    {
        private static readonly string ApiKey = "FwFiSEZx7ZGnx8VobX6VOq6Y4oKXMbDA";

        public string Trigger => "!gif";

        public void Action(IBot bot, string channel, string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                bot.SendMessage($"Usage: {Trigger} <search>", channel);
            }
            else
            {
                var giphy = new Giphy(ApiKey);

                Task<GiphySearchResult> task = Task.Run(async () =>
                    await giphy.GifSearch(new SearchParameter
                    {
                        Query = args
                    }
                ));

                if (task.Result?.Data[0] != null)
                {
                    bot.SendMessage(task.Result.Data[0].EmbedUrl, channel);
                }
                else
                {
                    bot.SendMessage($"Nothing found for {args}", channel);
                }
            }
        }
    }
}