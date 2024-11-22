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
        Task<ServiceResult> AddUserToActivityAsync(int activityId, int userId);
        Task<ServiceResult> RemoveUserFromActivityAsync(int activityId, int userId);
        Task<ServiceResult> CreateActivityWithCoordinatesAsync(ActivityRequestWithCoordinates activityRequest);
        Task<IEnumerable<ActivityWithParticipants>> GetUserActivitiesWithParticipantsAsync(int userId);
    }
}
