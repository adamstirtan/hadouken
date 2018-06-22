using Microsoft.EntityFrameworkCore;

namespace Hadouken.Database
{
	public sealed class HadoukenContext : DbContext
	{
		public DbSet<Message> Messages { get; set; }
		public DbSet<Quote> Quotes { get; set; }
		
		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			builder.UseSqlite("Data Source=bot.db");
		}
	}
}