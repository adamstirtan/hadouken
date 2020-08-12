using Hadouken.ObjectModel;

namespace Hadouken.Services
{
    public interface IQuoteService
    {
        public Quote AddQuote(Quote quote);

        public Quote GetRandomQuote();

        public Quote Search(string criteria);
    }
}