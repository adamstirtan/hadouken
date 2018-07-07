using System.Collections.Generic;
using System.Linq;

using Hadouken.Contracts;
using Hadouken.Database;

namespace Hadouken.Commands
{
	public sealed class TalkCommand : ICommand
	{
		public string Trigger => "!talk";

        public void Action(IBot bot, string channel, string args)
        {
        	if (string.IsNullOrEmpty(args))
            {
                bot.Client.SendMessage($"Usage: {Trigger} <nick>", channel);
            }
            else
            {
            	var split = args.Split(" ");
            	
            	if (split.Length != 1)
            	{
            		bot.Client.SendMessage($"Usage: {Trigger} <nick>", channel);
            		return;
            	}
            	
            	List<Message> messages;
            	
            	using (var db = new HadoukenContext())
            	{
            		messages = db.Messages
            			.Where(x => x.Nick == split[0])
            			.ToList();
            	}
            	
            	if (messages.Count < 5)
            	{
            		bot.Client.SendMessage($"{split[0]} hasn't said enough to make them talk", channel);
            		return;
            	}
            }
        }
	}
}