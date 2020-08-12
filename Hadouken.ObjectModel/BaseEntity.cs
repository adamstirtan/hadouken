using System;

namespace Hadouken.ObjectModel
{
	public abstract class BaseEntity
	{
		public int Id { get; set; }

		public DateTime Created { get; set; }
	}
}