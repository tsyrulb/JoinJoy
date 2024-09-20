using JoinJoy.Core.Interfaces;
using JoinJoy.Infrastructure.Data.Repositories;
using JoinJoy.Infrastructure.Data;
using JoinJoy.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddAsync(Message entity)
    {
        await _context.Messages.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Message> entities)
    {
        await _context.Messages.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Message>> FindAsync(Expression<Func<Message, bool>> predicate)
    {
        return await _context.Messages.Where(predicate).ToListAsync();
    }

    // Retrieve messages for a specific conversation
    public async Task<IEnumerable<Message>> GetMessagesForConversationAsync(int conversationId)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetMessagesForUserAsync(int userId)
    {
        return await _context.Messages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task RemoveAsync(Message entity)
    {
        _context.Messages.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<Message> entities)
    {
        _context.Messages.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Message entity)
    {
        _context.Messages.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _context.Messages.ToListAsync();
    }

    public async Task<Message> GetByIdAsync(int id)
    {
        return await _context.Messages.FindAsync(id);
    }
}
