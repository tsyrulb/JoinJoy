using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Services
{
    public class UserUnavailabilityService : IUserUnavailabilityService
    {
        private readonly IRepository<UserUnavailability> _userUnavailabilityRepository;
        private readonly IUserRepository _userRepository;

        public UserUnavailabilityService(IRepository<UserUnavailability> userUnavailabilityRepository, IUserRepository userRepository)
        {
            _userUnavailabilityRepository = userUnavailabilityRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResult> AddUnavailabilityAsync(int userId, UserUnavailabilityRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            var newUnavailability = new UserUnavailability
            {
                UserId = userId,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            await _userUnavailabilityRepository.AddAsync(newUnavailability);
            await _userUnavailabilityRepository.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Unavailability added successfully" };
        }

        public async Task<ServiceResult> RemoveUnavailabilityAsync(int userId, int unavailabilityId)
        {
            var unavailability = await _userUnavailabilityRepository.GetByIdAsync(unavailabilityId);
            if (unavailability == null || unavailability.UserId != userId)
            {
                return new ServiceResult { Success = false, Message = "Unavailability not found or unauthorized" };
            }

            await _userUnavailabilityRepository.RemoveAsync(unavailability);
            await _userUnavailabilityRepository.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Unavailability removed successfully" };
        }

        public async Task<IEnumerable<UserUnavailability>> GetUnavailabilitiesAsync(int userId)
        {
            return await _userUnavailabilityRepository.FindAsync(u => u.UserId == userId);
        }
    }
}
