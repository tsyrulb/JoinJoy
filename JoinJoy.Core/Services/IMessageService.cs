using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Services
{
    public interface IMessageService
    {
        Task<ServiceResult> SendMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesForConversationAsync(int conversationId);
        Task<ServiceResult> MarkMessagesAsReadAsync(int conversationId, int userId);
        Task<ServiceResult> DeleteMessageAsync(int messageId);
        Task<ServiceResult> DeleteConversationAsync(int conversationId);
        Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId);
    }
}
