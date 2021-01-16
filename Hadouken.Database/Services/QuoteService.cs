using Hadouken.ObjectModel;

namespace Hadouken.Database.Services
{
    public class QuoteService : BaseService<Quote>, IQuoteService
    {
        public QuoteService(ApplicationDbContext context)
            : base(context)
        { }
    }
}