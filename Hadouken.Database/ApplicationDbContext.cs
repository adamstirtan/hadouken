using Microsoft.EntityFrameworkCore;

using Hadouken.ObjectModel;

namespace Hadouken.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Quote> Quotes { get; set; }
    }
}