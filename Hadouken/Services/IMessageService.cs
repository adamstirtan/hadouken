using Hadouken.Database;

namespace Hadouken.Services
{
    public interface IMessageService
    {
        Message AddMessage(Message message);
    }
}