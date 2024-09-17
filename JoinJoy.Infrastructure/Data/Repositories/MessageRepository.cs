using JoinJoy.Core.Interfaces;
using JoinJoy.Infrastructure.Data.Repositories;
using JoinJoy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Retrieve messages for a specific conversation
    public async Task<IEnumerable<Message>> GetMessagesForConversationAsync(int conversationId)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.Timestamp)  // Optional: Order by the timestamp
            .ToListAsync();
    }

    public Task<IEnumerable<Message>> GetMessagesForUserAsync(int userId)
    {
        throw new NotImplementedException();
    }
}
