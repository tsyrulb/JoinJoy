using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                .Include(a => a.Location) // Include location if needed
                .Include(a => a.UserActivities)
                .ThenInclude(ua => ua.User)
                .ToListAsync();
        }

        public async Task<Activity> GetByIdWithUsersAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.Location) // Include location if needed
                .Include(a => a.UserActivities)
                .ThenInclude(ua => ua.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }


        public async Task<IEnumerable<TEntity>> FindWithRelatedAsync<TEntity>(
    Expression<Func<TEntity, bool>> predicate,
    params Expression<Func<TEntity, object>>[] includes
) where TEntity : class
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(predicate).ToListAsync();
        }
    }
}
