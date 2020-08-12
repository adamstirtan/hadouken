using System;

using Hadouken.Database.Repositories;
using Hadouken.ObjectModel;

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