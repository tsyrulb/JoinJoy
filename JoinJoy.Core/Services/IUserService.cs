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
        Task<ServiceResult> AddUserSubcategoriesAsync(int userId, List<UserSubcategoryDto> subcategoryIds);
        Task<ServiceResult> RemoveUserSubcategoryAsync(int userId, int subcategoryId);
        Task<ServiceResult> UpdateUserDetailsAsync(int userId, string? name, string? email, string? password, string? profilePhoto, DateTime? dateOfBirth, Location? location);
        Task<ServiceResult> UpdateUserDetailsAsync(int userId, string? name, string? email, string? password, string? profilePhoto, DateTime? dateOfBirth, string? address);
        Task<ServiceResult> UpdateUserDistanceWillingToTravelAsync(int userId, double distance);
        Task<IEnumerable<UserSubcategory>> GetSubcategoriesByUserIdAsync(int userId);
        Task<bool> IsUserAvailableAsync(int userId, DateTime currentTime);
        Task<ServiceResult> SetUserAvailabilityAsync(int userId, Models.DayOfWeek unavailableDay, TimeSpan unavailableStartTime, TimeSpan unavailableEndTime);
    }
}
