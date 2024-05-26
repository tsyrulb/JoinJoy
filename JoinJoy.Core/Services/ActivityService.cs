using JoinJoy.Core.Models;
using JoinJoy.Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JoinJoy.Core.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IRepository<Activity> _activityRepository;

        public ActivityService(IRepository<Activity> activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<ServiceResult> CreateActivityAsync(Activity activity)
        {
            // Implement create activity logic here
            return new ServiceResult { Success = true, Message = "Activity created successfully" };
        }

        public async Task<IEnumerable<Activity>> GetActivitiesAsync()
        {
            return await _activityRepository.GetAllAsync();
        }

        public async Task<ServiceResult> DeleteActivityAsync(int activityId)
        {
            // Implement delete activity logic here
            return new ServiceResult { Success = true, Message = "Activity deleted successfully" };
        }
    }
}
