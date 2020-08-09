namespace Hadouken.Database.Repositories
{
    public sealed class QuoteRepository : GenericRepository<Quote>, IQuoteRepository
    {
        public QuoteRepository(HadoukenContext context)
            : base(context)
        { }
    }
}