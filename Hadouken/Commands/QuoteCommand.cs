using System.Linq;

using Hadouken.Contracts;

namespace Hadouken.Commands
{
	public class QuoteCommand : ICommand
	{
		public string Trigger => "!quote";
		
		public void Action(IBot bot, string channel, string args)
		{
			if (string.IsNullOrEmpty(args))
			{
				using (var db = new HadoukenContext())
				{
					var quote = db.Quotes.FirstOrDefault();
				}
				
				if (quote == null)
				{
					bot.Client.SendMessage("There are no quotes to display", channel);
				}
				else
				{
					bot.Client.SendMessage($"{quote.Nick}: {quote.Content}", channel);
				}
			}
			else
			{
				var arg = args.Split(" ");
				
				if (arg.ToLower().Equals("-a"))
				{
					
				}
			}
		}
	}
}