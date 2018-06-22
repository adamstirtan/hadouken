using System;

namespace Hadouken.Database
{
	public abstract class BaseEntity
	{
		public int Id { get; set; }
		public DateTime Created { get; set; }
	}
}