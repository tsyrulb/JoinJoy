using JoinJoy.Core.Models;
using JoinJoy.Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JoinJoy.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> _messageRepository;

        public MessageService(IRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<ServiceResult> SendMessageAsync(Message message)
        {
            // Implement send message logic here
            return new ServiceResult { Success = true, Message = "Message sent successfully" };
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(int userId)
        {
            return await _messageRepository.GetAllAsync();
        }
    }
}
