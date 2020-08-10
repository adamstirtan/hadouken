using System;

using Hadouken.Database;
using Hadouken.Database.Repositories;

namespace Hadouken.Services
{
    public sealed class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Message AddMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}