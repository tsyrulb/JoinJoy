using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IAdminService
    {
        Task<ServiceResult> PromoteUserToAdminAsync(int userId);
        Task<ServiceResult> DeactivateUserAsync(int userId);
        Task<ServiceResult> DeleteActivityAsync(int activityId);
        Task<ServiceResult> DeleteFeedbackAsync(int feedbackId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
    }
}
