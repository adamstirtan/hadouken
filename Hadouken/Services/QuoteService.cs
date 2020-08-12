using System;
using System.Linq;

using Hadouken.Database.Repositories;
using Hadouken.ObjectModel;

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
            return null;
            //return _quoteRepository
            //    .Where(x => string.Contains(criteria, StringComparison.InvariantCultureIgnoreCase))
            //    .FirstOrDefault();
        }
    }
}