using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;

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


        public MatchingService(
            IRepository<User> userRepository,
            IRepository<Match> matchRepository,
            IRepository<Activity> activityRepository,
            IRepository<UserSubcategory> userSubcategoryRepository,
            IRepository<UserActivity> userActivity,
            IRepository<Subcategory> subcategory,
            IRepository<Category> category)
        {
            _userRepository = userRepository;
            _matchRepository = matchRepository;
            _activityRepository = activityRepository;
            _userSubcategoryRepository = userSubcategoryRepository;
            _userActivity = userActivity;
            _subcategory = subcategory;
            _category = category;
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


    }
}
