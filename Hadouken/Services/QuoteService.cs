using System;
using System.Linq;

using Hadouken.Database;
using Hadouken.Database.Repositories;

namespace Hadouken.Services
{
    public sealed class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteService(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public Quote AddQuote(Quote quote)
        {
            return _quoteRepository.Create(quote);
        }

        public Quote GetRandomQuote()
        {
            return _quoteRepository
                .All()
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefault();
        }

        public Quote Search(string criteria)
        {
            //return _quoteRepository
            //    .Where(x => string.Contains(criteria, StringComparison.InvariantCultureIgnoreCase))
            //    .FirstOrDefault();
        }
    }
}