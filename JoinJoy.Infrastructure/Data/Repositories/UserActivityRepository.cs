using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class UserActivityRepository : Repository<UserActivity>, IUserActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public UserActivityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserActivity>> GetByUserIdAsync(int userId)
        {
            return await _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserActivity>> GetByActivityIdAsync(int activityId)
        {
            return await _context.UserActivities
                .Where(ua => ua.ActivityId == activityId)
                .ToListAsync();
        }
        public async Task<UserActivity> GetByUserAndActivityIdAsync(int userId, int activityId)
        {
            return await _context.UserActivities
                .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ActivityId == activityId);
        }

        // This method retrieves all users who participated in a specific activity
        public async Task<IEnumerable<int>> GetUsersInActivityAsync(int activityId)
        {
            return await _context.UserActivities
                                 .Where(ua => ua.ActivityId == activityId)
                                 .Select(ua => ua.UserId)
                                 .ToListAsync();
        }
    }
}
