using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IUserActivityRepository : IRepository<UserActivity>
    {
        Task<IEnumerable<UserActivity>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserActivity>> GetByActivityIdAsync(int activityId);
    }
}
