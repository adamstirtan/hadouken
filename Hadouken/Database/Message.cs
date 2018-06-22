using System;

namespace Hadouken.Database
{
	public class Message
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public string Nick { get; set; }
		public DateTime Created { get; set; }
	}
}