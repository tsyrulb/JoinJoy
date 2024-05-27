using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IAvailabilityService
    {
        Task<IEnumerable<Availability>> GetAllAvailabilitiesAsync();
        Task<Availability> GetAvailabilityByIdAsync(int id);
        Task<ServiceResult> AddAvailabilityAsync(Availability availability);
        Task<ServiceResult> UpdateAvailabilityAsync(Availability availability);
        Task<ServiceResult> DeleteAvailabilityAsync(int id);
    }
}
