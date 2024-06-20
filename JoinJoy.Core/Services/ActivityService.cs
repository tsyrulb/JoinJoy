using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JoinJoy.Infrastructure.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IRepository<Activity> _activityRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<UserActivity> _userActivityRepository;
        private readonly ILocationRepository _customLocationRepository;
        private readonly string _googleApiKey;

        public ActivityService(
            IRepository<Activity> activityRepository,
            IRepository<Location> locationRepository,
            IUserRepository userRepository,
            IRepository<UserActivity> userActivityRepository,
            ILocationRepository customLocationRepository,
            string googleApiKey
        )
        {
            _activityRepository = activityRepository;
            _locationRepository = locationRepository;
            _userRepository = userRepository;
            _userActivityRepository = userActivityRepository;
            _customLocationRepository = customLocationRepository;
            _googleApiKey = googleApiKey ?? throw new ArgumentNullException(nameof(googleApiKey));
        }

        public async Task<ServiceResult> CreateActivityAsync(ActivityRequest activityRequest)
        {
            var (latitude, longitude, placeId) = await GetCoordinatesAsync(activityRequest.LocationName);
            var newLocation = new Location
            {
                Latitude = latitude,
                Longitude = longitude,
                Address = activityRequest.LocationName,
                PlaceId = placeId
            };

            await _locationRepository.AddAsync(newLocation);

            var activity = new Activity
            {
                Name = activityRequest.Name,
                Description = activityRequest.Description,
                Date = activityRequest.Date,
                Location = newLocation,
                CreatedById = activityRequest.CreatedById
            };

            await _activityRepository.AddAsync(activity);
            // Add the creator to UserActivities
            var userActivity = new UserActivity
            {
                UserId = activityRequest.CreatedById,
                ActivityId = activity.Id
            };

            await _userActivityRepository.AddAsync(userActivity);

            return new ServiceResult { Success = true, Message = "Activity created successfully" };
        }

        public async Task<ServiceResult> UpdateActivityAsync(int activityId, ActivityRequest activityRequest)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null)
            {
                return new ServiceResult { Success = false, Message = "Activity not found" };
            }

            if (activity.CreatedById != activityRequest.CreatedById)
            {
                return new ServiceResult { Success = false, Message = "User is not authorized to update this activity" };
            }

            // Update the activity properties
            activity.Name = activityRequest.Name;
            activity.Description = activityRequest.Description;
            activity.Date = activityRequest.Date;

            // Handle location update
            var (latitude, longitude, placeId) = await GetCoordinatesAsync(activityRequest.LocationName);
            var newLocation = new Location
            {
                Latitude = latitude,
                Longitude = longitude,
                Address = activityRequest.LocationName,
                PlaceId = placeId
            };

            await _locationRepository.AddAsync(newLocation);
            activity.Location = newLocation;

            await _activityRepository.UpdateAsync(activity);

            // Check if the old location is still referenced by any activity
            if (!await _customLocationRepository.IsLocationReferencedAsync(activity.LocationId))
            {
                var oldLocation = await _locationRepository.GetByIdAsync(activity.LocationId);
                if (oldLocation != null)
                {
                    await _locationRepository.RemoveAsync(oldLocation);
                }
            }

            return new ServiceResult { Success = true, Message = "Activity updated successfully" };
        }

        public async Task<ServiceResult> DeleteActivityAsync(int activityId)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null)
            {
                return new ServiceResult { Success = false, Message = "Activity not found" };
            }

            var locationId = activity.LocationId;

            await _activityRepository.RemoveAsync(activity);

            // Check if the location is still referenced by any activity
            if (!await _customLocationRepository.IsLocationReferencedAsync(locationId))
            {
                var location = await _locationRepository.GetByIdAsync(locationId);
                if (location != null)
                {
                    await _locationRepository.RemoveAsync(location);
                }
            }

            return new ServiceResult { Success = true, Message = "Activity deleted successfully" };
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _activityRepository.GetAllAsync();
        }

        public async Task<Activity> GetActivityByIdAsync(int activityId)
        {
            return await _activityRepository.GetByIdAsync(activityId);
        }
        public async Task<ServiceResult> AddUsersToActivityAsync(int activityId, List<int> userIds)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null)
            {
                return new ServiceResult { Success = false, Message = "Activity not found" };
            }

            foreach (var userId in userIds)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new ServiceResult { Success = false, Message = $"User with ID {userId} not found" };
                }

                var userActivity = new UserActivity
                {
                    UserId = userId,
                    ActivityId = activityId
                };

                await _userActivityRepository.AddAsync(userActivity);
            }

            return new ServiceResult { Success = true, Message = "Users added to activity successfully" };
        }
        private async Task<(double latitude, double longitude, string placeId)> GetCoordinatesAsync(string address)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key={1}&sensor=false", Uri.EscapeDataString(address), _googleApiKey);

            try
            {
                WebRequest request = WebRequest.Create(requestUri);
                WebResponse response = await request.GetResponseAsync();

                XDocument xdoc = XDocument.Load(response.GetResponseStream());

                XElement result = xdoc.Element("GeocodeResponse").Element("result");
                XElement locationElement = result.Element("geometry").Element("location");
                XElement lat = locationElement.Element("lat");
                XElement lng = locationElement.Element("lng");
                XElement placeIdElement = result.Element("place_id");

                double latitude = double.Parse(lat.Value);
                double longitude = double.Parse(lng.Value);
                string placeId = placeIdElement.Value;

                return (latitude, longitude, placeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
