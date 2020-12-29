using Hadouken.ObjectModel;

namespace Hadouken.Database.Services
{
    public class MessageService : BaseService<Message>, IMessageService
    {
        public MessageService(ApplicationDbContext context)
            : base(context)
        { }
    }
}