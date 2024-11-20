using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Services
{
    public interface IMatchingService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync();
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<IEnumerable<Category>> GetAllCategoryAsync();

        Task<User> GetUserWithDetailsAsync(int userId);
        Task<IEnumerable<Subcategory>> GetAllSubcategoryAsync();
        Task<IEnumerable<UserSubcategory>> GetAllUserSubcategoryAsync();
        Task<IEnumerable<UserRecommendation>> GetRecommendedUsersForActivityAsync(int activityId, int topN);
        Task<IEnumerable<ActivityRecommendation>> GetRecommendedActivitiesForUserAsync(int userId, int topN);
        Task<ServiceResult> SendInvitationsAsync(int senderId, int activityId, List<int> receiverIds);
        Task<ServiceResult> AcceptInvitationAsync(int matchId, int userId);
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<Match> GetMatchByIdAsync(int id);
        Task<ServiceResult> CreateMatchAsync(Match match);
        Task<ServiceResult> UpdateMatchAsync(int id, Match updatedMatch);
        Task<ServiceResult> DeleteMatchAsync(int id);
    }
}