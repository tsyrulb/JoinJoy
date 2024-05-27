using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IActivityPreferenceService
    {
        Task<IEnumerable<ActivityPreference>> GetAllActivityPreferencesAsync();
        Task<ActivityPreference> GetActivityPreferenceByIdAsync(int id);
        Task<ServiceResult> AddActivityPreferenceAsync(ActivityPreference activityPreference);
        Task<ServiceResult> UpdateActivityPreferenceAsync(ActivityPreference activityPreference);
        Task<ServiceResult> DeleteActivityPreferenceAsync(int id);
    }
}
