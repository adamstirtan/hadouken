using System;
using System.Linq;

using Hadouken.Contracts;
using Hadouken.Database;

namespace Hadouken.Commands
{
	public class QuoteCommand : ICommand
	{
		public string Trigger => "!quote";
		
		public void Action(IBot bot, string channel, string args)
		{
			//if (string.IsNullOrEmpty(args))
			//{
			//	Quote quote;

   //             using (var db = new HadoukenContext())
   //             {
   //                 quote = db.Quotes
   //                     .OrderBy(x => Guid.NewGuid())
   //                     .FirstOrDefault();
   //             }

   //             if (quote == null)
			//	{
			//		bot.Client.SendMessage("There are no quotes to display", channel);
			//	}
			//	else
			//	{
			//		bot.Client.SendMessage($"{quote.Nick}: {quote.Content}", channel);
			//	}
			//}
			//else
			//{
			//	var split = args.Split(" ");
                
   //             if (split.Length < 3)
   //             {
   //                 bot.Client.SendMessage("Usage: !quote | !quote add <nick> <content>", channel);
   //             }
   //             else if (split[0].ToLower().Equals("add"))
			//	{
   //                 using (var db = new HadoukenContext())
   //                 {
   //                     var quote = new Quote
   //                     {
   //                         Nick = split[1],
   //                         Content = string.Join(" ", split.Skip(2)),
   //                         Created = DateTime.UtcNow
   //                     };

   //                     db.Quotes.Add(quote);

   //                     if (db.SaveChanges() > 0)
   //                     {
   //                         bot.Client.SendMessage($"Added quote #{quote.Id}", channel);
   //                     }
   //                     else
   //                     {
   //                         bot.Client.SendMessage("There was a problem saving that quote, blame rhaydeo.", channel);
   //                     }
   //                 }
   //             }
			//}
		}
	}
}