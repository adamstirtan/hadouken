using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Hadouken.Common;
using Hadouken.Contracts;

namespace Hadouken.Commands
{
    public class UrbanDictionaryCommand : ICommand
    {
        public string Trigger => "!ud";

        public void Action(IBot bot, string channel, string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                bot.Client.SendMessage($"Usage: {Trigger} <search>", channel);
            }
            else
            {
                var result = string.Empty;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", "Hadouken IRC Bot");

                    Task<string> task = Task.Run(async () =>
                        await client.GetStringAsync(
                            $"http://api.urbandictionary.com/v0/define?term={args}"));
                    
                    result = task.Result;
                }

                if (string.IsNullOrEmpty(result))
                {
                    bot.Client.SendMessage($"Nothing found for {args}", channel);
                }
                else
                {
                    dynamic json = JObject.Parse(result);

                    if (json.list != null &&
                        json.list[0] != null &&
                        json.list[0].definition != null)
                    {
                        bot.Client.SendMessage($"{args}: {json.list[0].definition}", channel);
                    }
                    else
                    {
                        bot.Client.SendMessage($"Nothing found for {args}", channel);
                    }
                }
            }
        }
    }
}
