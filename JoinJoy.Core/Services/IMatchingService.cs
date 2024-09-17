using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Services
{
    public interface IMatchingService
    {
        Task<IEnumerable<Match>> FindMatchesAsync();

        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync();
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<IEnumerable<Category>> GetAllCategoryAsync();

        Task<User> GetUserWithDetailsAsync(int userId);
        Task<IEnumerable<Subcategory>> GetAllSubcategoryAsync();
        Task<IEnumerable<UserSubcategory>> GetAllUserSubcategoryAsync();
    }
}