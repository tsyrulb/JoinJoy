using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Repositories;

namespace JoinJoy.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessageAsync(Message message)
        {
            await _messageRepository.AddAsync(message);
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(int userId)
        {
            return await _messageRepository.GetMessagesByUserIdAsync(userId);
        }
    }
}