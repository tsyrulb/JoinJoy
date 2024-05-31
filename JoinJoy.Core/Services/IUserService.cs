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
        Task<User> GetUserByIdAsync(int userId);
        Task<ServiceResult> DeleteUserAsync(int userId);
        Task<ServiceResult> AddUserSubcategoriesAsync(int userId, List<int> subcategoryIds);
        Task<ServiceResult> RemoveUserSubcategoryAsync(int userId, int subcategoryId);
        Task<ServiceResult> AddUserPreferredDestinationsAsync(int userId, List<UserPreferredDestination> preferredDestinations);
        Task<ServiceResult> AddUserAvailabilitiesAsync(int userId, List<UserAvailability> availabilities);
    }
}
