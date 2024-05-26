using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Services
{
    public interface IActivityService
    {
        Task<ServiceResult> CreateActivityAsync(Activity activity);
        Task<IEnumerable<Activity>> GetActivitiesAsync();
        Task<ServiceResult> DeleteActivityAsync(int activityId);
    }
}
