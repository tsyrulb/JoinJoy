using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesForUserAsync(int userId)
        {
            return await _context.ChatMessages
                .Where(cm => cm.SenderId == userId || cm.ReceiverId == userId)
                .ToListAsync();
        }
    }
}
