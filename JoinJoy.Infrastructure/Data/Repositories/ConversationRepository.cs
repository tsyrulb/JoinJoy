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

        public async Task SaveChangesAsync()
        {
            // Save the changes asynchronously to the database
            await _context.SaveChangesAsync();
        }
        public async Task<Conversation> FindExistingConversationAsync(int userId1, int userId2)
        {
            return await _context.Conversations
                .Where(c => c.Participants.Any(p => p.UserId == userId1) && c.Participants.Any(p => p.UserId == userId2))
                .FirstOrDefaultAsync();
        }
    }

}
