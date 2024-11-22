using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Interfaces
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Task<IEnumerable<Activity>> GetAllWithUsersAsync();
        Task<Activity> GetByIdWithUsersAsync(int id);
        Task<IEnumerable<TEntity>> FindWithRelatedAsync<TEntity>(
    Expression<Func<TEntity, bool>> predicate,
    params Expression<Func<TEntity, object>>[] includes) where TEntity : class;
    }
}
