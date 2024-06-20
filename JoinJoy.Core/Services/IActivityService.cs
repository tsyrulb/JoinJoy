using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IActivityService
    {
        Task<ServiceResult> CreateActivityAsync(ActivityRequest activityRequest);
        Task<ServiceResult> UpdateActivityAsync(int activityId, ActivityRequest activityRequest);
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<Activity> GetActivityByIdAsync(int activityId);
        Task<ServiceResult> DeleteActivityAsync(int activityId);
        Task<ServiceResult> AddUsersToActivityAsync(int activityId, List<int> userIds);
    }
}
