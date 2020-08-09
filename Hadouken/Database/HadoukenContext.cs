using Microsoft.EntityFrameworkCore;

namespace Hadouken.Database
{
    public sealed class HadoukenContext : DbContext
    {
        public HadoukenContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Quote> Quotes { get; set; }
    }
}