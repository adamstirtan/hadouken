using Hadouken.ObjectModel;

namespace Hadouken.Database.Services
{
    public interface IQuoteService : IService<Quote>, IServiceAsync<Quote>
    { }
}