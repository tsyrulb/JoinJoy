using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Services
{
    public class ActivityPreferenceService : IActivityPreferenceService
    {
        private readonly IRepository<ActivityPreference> _activityPreferenceRepository;

        public ActivityPreferenceService(IRepository<ActivityPreference> activityPreferenceRepository)
        {
            _activityPreferenceRepository = activityPreferenceRepository;
        }

        public async Task<IEnumerable<ActivityPreference>> GetAllActivityPreferencesAsync()
        {
            return await _activityPreferenceRepository.GetAllAsync();
        }

        public async Task<ActivityPreference> GetActivityPreferenceByIdAsync(int id)
        {
            return await _activityPreferenceRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> AddActivityPreferenceAsync(ActivityPreference activityPreference)
        {
            await _activityPreferenceRepository.AddAsync(activityPreference);
            return new ServiceResult { Success = true, Message = "Activity Preference added successfully" };
        }

        public async Task<ServiceResult> UpdateActivityPreferenceAsync(ActivityPreference activityPreference)
        {
            await _activityPreferenceRepository.UpdateAsync(activityPreference);
            return new ServiceResult { Success = true, Message = "Activity Preference updated successfully" };
        }

        public async Task<ServiceResult> DeleteActivityPreferenceAsync(int id)
        {
            var activityPreference = await _activityPreferenceRepository.GetByIdAsync(id);
            if (activityPreference != null)
            {
                await _activityPreferenceRepository.RemoveAsync(activityPreference);
                return new ServiceResult { Success = true, Message = "Activity Preference deleted successfully" };
            }
            return new ServiceResult { Success = false, Message = "Activity Preference not found" };
        }
    }
}
