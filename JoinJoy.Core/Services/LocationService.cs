using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> AddLocationAsync(Location location)
        {
            await _locationRepository.AddAsync(location);
            return new ServiceResult { Success = true, Message = "Location added successfully" };
        }

        public async Task<ServiceResult> UpdateLocationAsync(int id, Location location)
        {
            var existingLocation = await _locationRepository.GetByIdAsync(id);
            if (existingLocation == null)
            {
                return new ServiceResult { Success = false, Message = "Location not found" };
            }

            existingLocation.Address = location.Address;
            existingLocation.Latitude = location.Latitude;
            existingLocation.Longitude = location.Longitude;

            await _locationRepository.UpdateAsync(existingLocation);
            return new ServiceResult { Success = true, Message = "Location updated successfully" };
        }

        public async Task<ServiceResult> DeleteLocationAsync(int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
            {
                return new ServiceResult { Success = false, Message = "Location not found" };
            }

            await _locationRepository.RemoveAsync(location);
            return new ServiceResult { Success = true, Message = "Location deleted successfully" };
        }
    }
}
