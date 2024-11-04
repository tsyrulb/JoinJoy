using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(int id);
        Task<ServiceResult> AddLocationAsync(Location location);
        Task<ServiceResult> UpdateLocationAsync(int id, Location location);
        Task<ServiceResult> DeleteLocationAsync(int id);
    }
}
