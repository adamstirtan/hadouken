using Hadouken.ObjectModel;

namespace Hadouken.Database.Services
{
    public interface IMessageService : IService<Message>, IServiceAsync<Message>
    { }
}