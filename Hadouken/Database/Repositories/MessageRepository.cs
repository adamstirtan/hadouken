namespace Hadouken.Database.Repositories
{
    public sealed class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(HadoukenContext context)
            : base(context)
        { }
    }
}