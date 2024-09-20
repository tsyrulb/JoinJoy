using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IConversationRepository : IRepository<Conversation>
    {
        Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId);
        Task<Conversation> GetConversationWithMessagesAsync(int conversationId);
        Task SaveChangesAsync();
        Task<Conversation> FindExistingConversationAsync(int userId1, int userId2);
    }
}
