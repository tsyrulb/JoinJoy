using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using JoinJoy.Infrastructure.Repositories;

namespace JoinJoy.Core.Services
{
    internal class UserService
    {
        public class UserService : IUserService
        {
            private readonly IUserRepository _userRepository;

            public UserService(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Result> RegisterUserAsync(UserRegistrationDto userDto)
            {
                // Registration logic here
                return new Result { Success = true };
            }

            public async Task<User> GetUserByIdAsync(int userId)
            {
                return await _userRepository.GetByIdAsync(userId);
            }
        }
    }
}
