using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace JoinJoy.Infrastructure.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Match> _matchRepository;
        private readonly IRepository<Activity> _activityRepository;
        private readonly IRepository<UserSubcategory> _userSubcategoryRepository;
        private readonly IRepository<UserActivity> _userActivity;
        private readonly IRepository<Subcategory> _subcategory;
        private readonly IRepository<Category> _category;
        private readonly HttpClient _httpClient;


        public MatchingService(
            IRepository<User> userRepository,
            IRepository<Match> matchRepository,
            IRepository<Activity> activityRepository,
            IRepository<UserSubcategory> userSubcategoryRepository,
            IRepository<UserActivity> userActivity,
            IRepository<Subcategory> subcategory,
            IRepository<Category> category,
            HttpClient httpClient)
        {
            _userRepository = userRepository;
            _matchRepository = matchRepository;
            _activityRepository = activityRepository;
            _userSubcategoryRepository = userSubcategoryRepository;
            _userActivity = userActivity;
            _subcategory = subcategory;
            _category = category;
            _httpClient = httpClient;
        }

        private double CalculateDistance(Location loc1, Location loc2)
        {
            double R = 6371e3; // metres
            double φ1 = loc1.Latitude.Value * Math.PI / 180;
            double φ2 = loc2.Latitude.Value * Math.PI / 180;
            double Δφ = (loc2.Latitude.Value - loc1.Latitude.Value) * Math.PI / 180;
            double Δλ = (loc2.Longitude.Value - loc1.Longitude.Value) * Math.PI / 180;

            double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                        Math.Cos(φ1) * Math.Cos(φ2) *
                        Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c; // in metres
            return d / 1000; // in kilometres
        }

        // Fetch a user with detailed activities and subcategories
        public async Task<User> GetUserWithDetailsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user != null)
            {
                var activities = await _activityRepository.GetAllAsync();
                var userSubcategoryList = await _userSubcategoryRepository.FindAsync(us => us.UserId == userId);
                user.UserSubcategories = userSubcategoryList.ToList();
                // Load the user's activities
                user.UserActivities = activities
                    .SelectMany(a => a.UserActivities)
                    .Where(ua => ua.UserId == userId)
                    .ToList();
            }

            return user;
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _activityRepository.GetAllAsync();

        }
        public async Task<IEnumerable<UserActivity>> GetAllUserActivitiesAsync()
        {
            return await _userActivity.GetAllAsync();

        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Eagerly load subcategories and activities for all users
            var users = await _userRepository.GetAllAsync();
            return users;
        }

        public async Task<IEnumerable<UserSubcategory>> GetAllUserSubcategoryAsync()
        {
            // Eagerly load subcategories and activities for all users
            return await _userSubcategoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Subcategory>> GetAllSubcategoryAsync()
        {
            // Eagerly load subcategories and activities for all users
            return await _subcategory.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            // Eagerly load subcategories and activities for all users
            return await _category.GetAllAsync();
        }

        public async Task<IEnumerable<UserRecommendation>> GetRecommendedUsersForActivityAsync(int activityId, int topN)
        {
            var flaskApiUrl = $"http://localhost:5000/recommend_users?activity_id={activityId}&top_n={topN}";

            try
            {
                var response = await _httpClient.GetAsync(flaskApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var recommendations = await response.Content.ReadFromJsonAsync<IEnumerable<UserRecommendation>>();
                    return recommendations ?? Enumerable.Empty<UserRecommendation>();
                }
                else
                {
                    throw new Exception($"Error fetching user recommendations: {response.StatusCode} {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception($"An error occurred while communicating with the Flask API: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ActivityRecommendation>> GetRecommendedActivitiesForUserAsync(int userId, int topN)
        {
            var flaskApiUrl = $"http://localhost:5000/recommend_activities?user_id={userId}&top_n={topN}";

            try
            {
                var response = await _httpClient.GetAsync(flaskApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var recommendations = await response.Content.ReadFromJsonAsync<IEnumerable<ActivityRecommendation>>();
                    return recommendations ?? Enumerable.Empty<ActivityRecommendation>();
                }
                else
                {
                    throw new Exception($"Error fetching activity recommendations: {response.StatusCode} {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception($"An error occurred while communicating with the Flask API: {ex.Message}", ex);
            }
        }
        public async Task<ServiceResult> SendInvitationsAsync(int senderId, int activityId, List<int> receiverIds)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null)
            {
                return new ServiceResult { Success = false, Message = "Activity not found" };
            }

            // Fetch all valid user IDs from the database
            var validUserIds = await _userRepository.GetAllAsync();
            var validReceiverIds = validUserIds.Select(u => u.Id).ToHashSet();

            // Validate receiverIds
            var invalidReceiverIds = receiverIds.Where(id => !validReceiverIds.Contains(id)).ToList();
            if (invalidReceiverIds.Any())
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Invalid user IDs: {string.Join(", ", invalidReceiverIds)}"
                };
            }

            var matches = new List<Match>();
            foreach (var receiverId in receiverIds)
            {
                // Check if a match already exists
                var existingMatch = await _matchRepository.FindAsync(m =>
                    m.ActivityId == activityId && m.UserId1 == senderId && m.User2Id == receiverId);

                if (!existingMatch.Any())
                {
                    // Load related entities
                    var sender = await _userRepository.GetByIdAsync(senderId);
                    var receiver = await _userRepository.GetByIdAsync(receiverId);

                    if (sender == null || receiver == null)
                    {
                        continue; // Skip if users are not found
                    }

                    var match = new Match
                    {
                        UserId1 = senderId,
                        User2Id = receiverId,
                        ActivityId = activityId,
                        MatchDate = DateTime.UtcNow,
                        IsAccepted = false, // Initially set to false until the user accepts the invitation
                        User1 = sender,
                        User2 = receiver,
                        Activity = activity // Assign the activity object
                    };
                    matches.Add(match);
                }
            }

            if (matches.Any())
            {
                foreach (var match in matches)
                {
                    await _matchRepository.AddAsync(match);
                }
            }

            return new ServiceResult
            {
                Success = true,
                Message = $"Invitations sent to {matches.Count} users."
            };
        }

        public async Task<ServiceResult> AcceptInvitationAsync(int matchId, int userId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
            {
                return new ServiceResult { Success = false, Message = "Match not found" };
            }

            if (match.User2Id != userId)
            {
                return new ServiceResult { Success = false, Message = "User is not authorized to accept this invitation" };
            }

            // Update the match to mark it as accepted
            match.IsAccepted = true;
            await _matchRepository.UpdateAsync(match);

            // Check if the user is already part of the activity in UserActivities
            var existingUserActivity = await _userActivity.FindAsync(ua =>
                ua.UserId == userId && ua.ActivityId == match.ActivityId);

            if (!existingUserActivity.Any())
            {
                // Add the user to the activity in UserActivities
                var userActivity = new UserActivity
                {
                    UserId = userId,
                    ActivityId = match.ActivityId
                };

                await _userActivity.AddAsync(userActivity);
            }

            return new ServiceResult { Success = true, Message = "Invitation accepted and user added to the activity successfully" };
        }


        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _matchRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Match>> GetMatchesByUserIdAsync(int userId)
        {
            var matches = await _matchRepository.FindAsync(m => m.UserId1 == userId || m.User2Id == userId);

            foreach (var match in matches)
            {
                // Explicitly load related entities if not already loaded
                match.User1 = await _userRepository.GetByIdAsync(match.UserId1);
                match.User2 = await _userRepository.GetByIdAsync(match.User2Id);
                match.Activity = await _activityRepository.GetByIdAsync(match.ActivityId);
            }

            return matches;
        }



        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _matchRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> CreateMatchAsync(Match match)
        {
            try
            {
                await _matchRepository.AddAsync(match);
                return new ServiceResult { Success = true, Message = "Match created successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error creating match: {ex.Message}" };
            }
        }

        public async Task<ServiceResult> UpdateMatchAsync(int id, Match updatedMatch)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
            {
                return new ServiceResult { Success = false, Message = "Match not found." };
            }

            match.UserId1 = updatedMatch.UserId1;
            match.User2Id = updatedMatch.User2Id;
            match.ActivityId = updatedMatch.ActivityId;
            match.IsAccepted = updatedMatch.IsAccepted;

            try
            {
                await _matchRepository.UpdateAsync(match);
                return new ServiceResult { Success = true, Message = "Match updated successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error updating match: {ex.Message}" };
            }
        }

        public async Task<ServiceResult> DeleteMatchAsync(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
            {
                return new ServiceResult { Success = false, Message = "Match not found." };
            }

            try
            {
                await _matchRepository.RemoveAsync(match);
                return new ServiceResult { Success = true, Message = "Match deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, Message = $"Error deleting match: {ex.Message}" };
            }
        }

    }
}
