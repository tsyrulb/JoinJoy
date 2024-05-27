using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatService(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task<ServiceResult> SendMessageAsync(ChatMessage chatMessage)
        {
            await _chatMessageRepository.AddAsync(chatMessage);
            return new ServiceResult { Success = true, Message = "Chat message sent successfully" };
        }

        public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(int userId)
        {
            return await _chatMessageRepository.GetMessagesForUserAsync(userId);
        }

        public Task<string> ProcessMessageAsync(ChatMessageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
