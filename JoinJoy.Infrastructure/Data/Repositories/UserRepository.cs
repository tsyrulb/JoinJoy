using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        private ApplicationDbContext ApplicationDbContext => _context as ApplicationDbContext;

        public async Task<User> GetUserWithSubcategoriesAsync(int userId)
        {
            return await ApplicationDbContext.Users
                .Include(u => u.UserSubcategories)
                .ThenInclude(us => us.Subcategory)
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<UserSubcategory>> GetUserSubcategoriesAsync(int userId)
        {
            return await ApplicationDbContext.UserSubcategories
                .Where(us => us.UserId == userId)
                .Include(us => us.Subcategory)
                .ToListAsync();
        }

        public async Task AddUserSubcategoryAsync(UserSubcategory userSubcategory)
        {
            await ApplicationDbContext.UserSubcategories.AddAsync(userSubcategory);
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task RemoveUserSubcategoryAsync(UserSubcategory userSubcategory)
        {
            ApplicationDbContext.UserSubcategories.Remove(userSubcategory);
            await ApplicationDbContext.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
