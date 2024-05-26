using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Services
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterUserAsync(User user);
        Task<ServiceResult> LoginAsync(string email, string password);
        Task<ServiceResult> UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<ServiceResult> DeleteUserAsync(int userId);
    }
}
