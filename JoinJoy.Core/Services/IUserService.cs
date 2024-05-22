using JoinJoy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    internal interface IUserService
    {
        Task<Result> RegisterUserAsync(UserRegistrationDto userDto);
        Task<User> GetUserByIdAsync(int userId);
        // Additional service methods
    }
}
