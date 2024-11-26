using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IUserUnavailabilityService
    {
        Task<ServiceResult> AddUnavailabilityAsync(int userId, UserUnavailabilityRequest request);
        Task<ServiceResult> RemoveUnavailabilityAsync(int userId, int unavailabilityId);
        Task<IEnumerable<UserUnavailability>> GetUnavailabilitiesAsync(int userId);
    }
}
