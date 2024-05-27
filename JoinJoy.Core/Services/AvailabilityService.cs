using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IRepository<Availability> _availabilityRepository;

        public AvailabilityService(IRepository<Availability> availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }

        public async Task<IEnumerable<Availability>> GetAllAvailabilitiesAsync()
        {
            return await _availabilityRepository.GetAllAsync();
        }

        public async Task<Availability> GetAvailabilityByIdAsync(int id)
        {
            return await _availabilityRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> AddAvailabilityAsync(Availability availability)
        {
            await _availabilityRepository.AddAsync(availability);
            return new ServiceResult { Success = true, Message = "Availability added successfully" };
        }

        public async Task<ServiceResult> UpdateAvailabilityAsync(Availability availability)
        {
            await _availabilityRepository.UpdateAsync(availability);
            return new ServiceResult { Success = true, Message = "Availability updated successfully" };
        }

        public async Task<ServiceResult> DeleteAvailabilityAsync(int id)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability != null)
            {
                await _availabilityRepository.RemoveAsync(availability);
                return new ServiceResult { Success = true, Message = "Availability deleted successfully" };
            }
            return new ServiceResult { Success = false, Message = "Availability not found" };
        }
    }
}
