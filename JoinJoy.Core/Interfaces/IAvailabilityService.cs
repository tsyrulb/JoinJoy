using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IAvailabilityService
    {
        Task<ServiceResult> AddOrUpdateAvailabilityAsync(int userId, List<AvailabilityRequest> availabilityRequests);
        Task<ServiceResult> DeleteAvailabilityAsync(int userId, int availabilityId);
        Task<IEnumerable<Availability>> GetUserAvailabilitiesAsync(int userId);
        Task<ServiceResult> CreateAvailabilityAsync(int userId, AvailabilityRequest availabilityRequest);
        Task<ServiceResult> UpdateAvailabilityAsync(int userId, AvailabilityRequest availabilityRequest);
    }
}
