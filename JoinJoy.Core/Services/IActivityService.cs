using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    internal interface IActivityService
    {
        Task<Activity> GetActivityByIdAsync(int activityId);
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        // Additional service methods
    }
}
