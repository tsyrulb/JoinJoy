using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Interfaces
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Task<IEnumerable<Activity>> GetAllWithUsersAsync();
        Task<Activity> GetByIdWithUsersAsync(int id);
    }
}
