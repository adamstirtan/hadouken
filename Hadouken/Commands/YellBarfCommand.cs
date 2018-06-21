using Hadouken.Contracts;

namespace Hadouken.Commands
{
	public sealed class YellBarfCommand : ICommand
	{
		public string Trigger => "!yellbarf";
		
		public void Action(IBot bot, string channel, string args)
		{
			bot.Client.SendMessage($"{args.ToUpper()}!!, channel);
		}
	}
}