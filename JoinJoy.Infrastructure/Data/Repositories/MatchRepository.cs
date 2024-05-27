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
    public class MatchRepository : Repository<Match>, IMatchRepository
    {
        public MatchRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Match>> GetMatchesForUserAsync(int userId)
        {
            return await _context.Matches
                .Where(m => m.UserId1 == userId || m.UserId2 == userId)
                .Include(m => m.User1)
                .Include(m => m.User2)
                .Include(m => m.Activity)
                .ToListAsync();
        }
    }
}
