using JoinJoy.Core.Models;
using JoinJoy.Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JoinJoy.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ServiceResult> RegisterUserAsync(User user)
        {
            // Implement registration logic here
            return new ServiceResult { Success = true, Message = "User registered successfully" };
        }

        public async Task<ServiceResult> LoginAsync(string email, string password)
        {
            // Implement login logic here
            return new ServiceResult { Success = true, Message = "User logged in successfully" };
        }

        public async Task<ServiceResult> UpdateUserAsync(User user)
        {
            // Implement update logic here
            return new ServiceResult { Success = true, Message = "User updated successfully" };
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<ServiceResult> DeleteUserAsync(int userId)
        {
            // Implement delete logic here
            return new ServiceResult { Success = true, Message = "User deleted successfully" };
        }
    }
}
