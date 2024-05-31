using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSubcategory> _userSubcategoryRepository;
        private readonly IRepository<UserPreferredDestination> _userPreferredDestinationRepository;
        private readonly IRepository<UserAvailability> _userAvailabilityRepository;

        public UserService(
            IRepository<User> userRepository,
            IRepository<UserSubcategory> userSubcategoryRepository,
            IRepository<UserPreferredDestination> userPreferredDestinationRepository,
            IRepository<UserAvailability> userAvailabilityRepository)
        {
            _userRepository = userRepository;
            _userSubcategoryRepository = userSubcategoryRepository;
            _userPreferredDestinationRepository = userPreferredDestinationRepository;
            _userAvailabilityRepository = userAvailabilityRepository;
        }

        public async Task<ServiceResult> RegisterUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
            return new ServiceResult { Success = true, Message = "User registered successfully" };
        }

        public async Task<ServiceResult> LoginAsync(string email, string password)
        {
            var user = await _userRepository.FindAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "Invalid email or password" };
            }

            return new ServiceResult { Success = true, Message = "User logged in successfully" };
        }

        public async Task<ServiceResult> UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
            return new ServiceResult { Success = true, Message = "User updated successfully" };
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<ServiceResult> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            await _userRepository.RemoveAsync(user);
            return new ServiceResult { Success = true, Message = "User deleted successfully" };
        }

        public async Task<ServiceResult> AddUserSubcategoriesAsync(int userId, List<int> subcategoryIds)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            foreach (var subcategoryId in subcategoryIds)
            {
                var userSubcategory = new UserSubcategory { UserId = userId, SubcategoryId = subcategoryId };
                await _userSubcategoryRepository.AddAsync(userSubcategory);
            }

            return new ServiceResult { Success = true, Message = "User subcategories added successfully" };
        }

        public async Task<ServiceResult> RemoveUserSubcategoryAsync(int userId, int subcategoryId)
        {
            var userSubcategoryList = await _userSubcategoryRepository.FindAsync(us => us.UserId == userId && us.SubcategoryId == subcategoryId);
            var userSubcategory = userSubcategoryList.FirstOrDefault();

            if (userSubcategory == null)
            {
                return new ServiceResult { Success = false, Message = "User subcategory not found" };
            }

            await _userSubcategoryRepository.RemoveAsync(userSubcategory);
            return new ServiceResult { Success = true, Message = "User subcategory removed successfully" };
        }

        public async Task<ServiceResult> AddUserPreferredDestinationsAsync(int userId, List<UserPreferredDestination> preferredDestinations)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            foreach (var destination in preferredDestinations)
            {
                destination.UserId = userId;
                await _userPreferredDestinationRepository.AddAsync(destination);
            }

            return new ServiceResult { Success = true, Message = "User preferred destinations added successfully" };
        }

        public async Task<ServiceResult> AddUserAvailabilitiesAsync(int userId, List<UserAvailability> availabilities)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            foreach (var availability in availabilities)
            {
                availability.UserId = userId;
                await _userAvailabilityRepository.AddAsync(availability);
            }

            return new ServiceResult { Success = true, Message = "User availabilities added successfully" };
        }
    }
}
