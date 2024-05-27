using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _activityRepository.GetAllAsync();
        }

        public async Task<Activity> GetActivityByIdAsync(int id)
        {
            return await _activityRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> CreateActivityAsync(Activity activity)
        {
            await _activityRepository.AddAsync(activity);
            return new ServiceResult { Success = true, Message = "Activity created successfully" };
        }

        public async Task<ServiceResult> UpdateActivityAsync(Activity activity)
        {
            await _activityRepository.UpdateAsync(activity);
            return new ServiceResult { Success = true, Message = "Activity updated successfully" };
        }

        public async Task<ServiceResult> DeleteActivityAsync(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity != null)
            {
                await _activityRepository.RemoveAsync(activity);
                return new ServiceResult { Success = true, Message = "Activity deleted successfully" };
            }
            return new ServiceResult { Success = false, Message = "Activity not found" };
        }

        public Task<IEnumerable<Activity>> GetActivitiesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
