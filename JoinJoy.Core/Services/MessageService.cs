using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<ServiceResult> SendMessageAsync(Message message)
        {
            await _messageRepository.AddAsync(message);
            return new ServiceResult { Success = true, Message = "Message sent successfully" };
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(int userId)
        {
            return await _messageRepository.GetMessagesForUserAsync(userId);
        }
    }
}
