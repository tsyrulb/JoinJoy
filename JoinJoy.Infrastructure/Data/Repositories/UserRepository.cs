using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Example of a custom method to get a user by email
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserInterests)
                .ThenInclude(ui => ui.Interest)
                .Include(u => u.UserHobbies)
                .ThenInclude(uh => uh.Hobby)
                .Include(u => u.UserActivityPreferences)
                .ThenInclude(uap => uap.ActivityPreference)
                .Include(u => u.UserPreferredDestinations)
                .ThenInclude(upd => upd.PreferredDestination)
                .Include(u => u.UserAvailabilities)
                .ThenInclude(ua => ua.Availability)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<IEnumerable<User>> GetUsersWithInterestsAsync()
        {
            throw new NotImplementedException();
        }

        // Example of a custom method to get a user with all related data
        public async Task<User> GetWithDetailsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserInterests)
                .ThenInclude(ui => ui.Interest)
                .Include(u => u.UserHobbies)
                .ThenInclude(uh => uh.Hobby)
                .Include(u => u.UserActivityPreferences)
                .ThenInclude(uap => uap.ActivityPreference)
                .Include(u => u.UserPreferredDestinations)
                .ThenInclude(upd => upd.PreferredDestination)
                .Include(u => u.UserAvailabilities)
                .ThenInclude(ua => ua.Availability)
                .Include(u => u.UserActivities)
                .Include(u => u.Matches)
                .Include(u => u.SentMessages)
                .Include(u => u.ReceivedMessages)
                .Include(u => u.Feedbacks)
                .Include(u => u.CreatedActivities)
                .Include(u => u.SentChatMessages)
                .Include(u => u.ReceivedChatMessages)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
