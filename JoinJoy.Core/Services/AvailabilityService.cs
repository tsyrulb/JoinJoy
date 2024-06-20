using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IRepository<Availability> _availabilityRepository;
        private readonly IRepository<UserAvailability> _userAvailabilityRepository;

        public AvailabilityService(
            IRepository<Availability> availabilityRepository,
            IRepository<UserAvailability> userAvailabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
            _userAvailabilityRepository = userAvailabilityRepository;
        }

        public async Task<ServiceResult> AddOrUpdateAvailabilityAsync(int userId, List<AvailabilityRequest> availabilityRequests)
        {
            foreach (var request in availabilityRequests)
            {
                if (request.Id.HasValue)
                {
                    // Update existing availability
                    var existingAvailability = await _availabilityRepository.GetByIdAsync(request.Id.Value);
                    if (existingAvailability != null)
                    {
                        existingAvailability.DayOfWeek = request.DayOfWeek;
                        existingAvailability.StartTime = request.StartTime;
                        existingAvailability.EndTime = request.EndTime;
                        await _availabilityRepository.UpdateAsync(existingAvailability);
                    }
                }
                else
                {
                    // Create new availability
                    var newAvailability = new Availability
                    {
                        DayOfWeek = request.DayOfWeek,
                        StartTime = request.StartTime,
                        EndTime = request.EndTime
                    };
                    await _availabilityRepository.AddAsync(newAvailability);

                    var userAvailability = new UserAvailability
                    {
                        UserId = userId,
                        AvailabilityId = newAvailability.Id
                    };
                    await _userAvailabilityRepository.AddAsync(userAvailability);
                }
            }

            return new ServiceResult { Success = true, Message = "Availabilities added/updated successfully" };
        }

        public async Task<ServiceResult> CreateAvailabilityAsync(int userId, AvailabilityRequest availabilityRequest)
        {
            var newAvailability = new Availability
            {
                DayOfWeek = availabilityRequest.DayOfWeek,
                StartTime = availabilityRequest.StartTime,
                EndTime = availabilityRequest.EndTime
            };
            await _availabilityRepository.AddAsync(newAvailability);

            var userAvailability = new UserAvailability
            {
                UserId = userId,
                AvailabilityId = newAvailability.Id
            };
            await _userAvailabilityRepository.AddAsync(userAvailability);

            return new ServiceResult { Success = true, Message = "Availability created successfully" };
        }

        public async Task<ServiceResult> UpdateAvailabilityAsync(int userId, AvailabilityRequest availabilityRequest)
        {
            if (!availabilityRequest.Id.HasValue)
            {
                return new ServiceResult { Success = false, Message = "Availability ID is required for update" };
            }

            var existingAvailability = await _availabilityRepository.GetByIdAsync(availabilityRequest.Id.Value);
            if (existingAvailability == null)
            {
                return new ServiceResult { Success = false, Message = "Availability not found" };
            }

            existingAvailability.DayOfWeek = availabilityRequest.DayOfWeek;
            existingAvailability.StartTime = availabilityRequest.StartTime;
            existingAvailability.EndTime = availabilityRequest.EndTime;
            await _availabilityRepository.UpdateAsync(existingAvailability);

            return new ServiceResult { Success = true, Message = "Availability updated successfully" };
        }

        public async Task<ServiceResult> DeleteAvailabilityAsync(int userId, int availabilityId)
        {
            var userAvailability = await _userAvailabilityRepository.FindAsync(ua => ua.UserId == userId && ua.AvailabilityId == availabilityId);
            var userAvailabilityToDelete = userAvailability.FirstOrDefault();
            if (userAvailabilityToDelete != null)
            {
                await _userAvailabilityRepository.RemoveAsync(userAvailabilityToDelete);

                // Check if the availability is used by other users
                var isUsedByOthers = await _userAvailabilityRepository.FindAsync(ua => ua.AvailabilityId == availabilityId && ua.UserId != userId);
                if (!isUsedByOthers.Any())
                {
                    var availability = await _availabilityRepository.GetByIdAsync(availabilityId);
                    if (availability != null)
                    {
                        await _availabilityRepository.RemoveAsync(availability);
                    }
                }
            }

            return new ServiceResult { Success = true, Message = "Availability deleted successfully" };
        }

        public async Task<IEnumerable<Availability>> GetUserAvailabilitiesAsync(int userId)
        {
            var userAvailabilities = await _userAvailabilityRepository.FindAsync(ua => ua.UserId == userId);
            var availabilityIds = userAvailabilities.Select(ua => ua.AvailabilityId).ToList();
            return await _availabilityRepository.FindAsync(a => availabilityIds.Contains(a.Id));
        }
    }
}
