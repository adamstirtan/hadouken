using System;

namespace Hadouken.Database
{
	public class Quote : BaseEntity
	{
		public string Nick { get; set; }
		public string Content { get; set; }
	}
}