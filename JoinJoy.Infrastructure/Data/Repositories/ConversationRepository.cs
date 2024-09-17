using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConversationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Get all conversations for a user
        public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId)
        {
            return await _context.Conversations
                .Include(c => c.Participants)
                .ThenInclude(uc => uc.User)
                .Where(c => c.Participants.Any(p => p.UserId == userId))
                .ToListAsync();
        }

        // Get a conversation by its ID, including all messages
        public async Task<Conversation> GetConversationWithMessagesAsync(int conversationId)
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                .Include(c => c.Participants)
                .ThenInclude(uc => uc.User)
                .SingleOrDefaultAsync(c => c.Id == conversationId);
        }
    }
}
