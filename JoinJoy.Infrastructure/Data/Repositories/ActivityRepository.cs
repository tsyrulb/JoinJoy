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
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Activity>> GetAllWithUsersAsync()
        {
            return await _context.Activities
                .Include(a => a.UserActivities)
                .ThenInclude(ua => ua.User)
                .ToListAsync();
        }

        public async Task<Activity> GetByIdWithUsersAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.UserActivities)
                .ThenInclude(ua => ua.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
