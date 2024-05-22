using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Infrastructure.Repositories;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    internal class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Activity> GetActivityByIdAsync(int activityId)
        {
            return await _activityRepository.GetByIdAsync(activityId);
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _activityRepository.GetAllAsync();
        }
    }
}
