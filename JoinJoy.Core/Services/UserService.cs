using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSubcategory> _userSubcategoryRepository;
        private readonly IRepository<UserPreferredDestination> _userPreferredDestinationRepository;
        private readonly IRepository<UserAvailability> _userAvailabilityRepository;
        private readonly string _googleApiKey;
        public UserService(
            IRepository<User> userRepository,
            IRepository<UserSubcategory> userSubcategoryRepository,
            IRepository<UserPreferredDestination> userPreferredDestinationRepository,
            IRepository<UserAvailability> userAvailabilityRepository,
            string googleApiKey
            )
        {
            _userRepository = userRepository;
            _userSubcategoryRepository = userSubcategoryRepository;
            _userPreferredDestinationRepository = userPreferredDestinationRepository;
            _userAvailabilityRepository = userAvailabilityRepository;
            _googleApiKey = googleApiKey ?? throw new ArgumentNullException(nameof(googleApiKey));
        }

        public async Task<ServiceResult> RegisterUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
            return new ServiceResult { Success = true, Message = "User registered successfully" };
        }

        public async Task<ServiceResult> LoginAsync(string email, string password)
        {
            var userList = await _userRepository.FindAsync(u => u.Email == email && u.Password == password);
            var user = userList.FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine("Invalid email or password");
                return new ServiceResult { Success = false, Message = "Invalid email or password" };
            }
            Console.WriteLine("User logged in successfully");
            return new ServiceResult { Success = true, Message = "User logged in successfully" };
        }

        public async Task<ServiceResult> UpdateUserDetailsAsync(int userId, string? name, string? email, string? password, string? profilePhoto, DateTime? dateOfBirth, string? address)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            // Update user properties if new values are provided
            if (!string.IsNullOrEmpty(name))
            {
                user.Name = name;
            }

            if (!string.IsNullOrEmpty(email))
            {
                user.Email = email;
            }

            if (!string.IsNullOrEmpty(password))
            {
                user.Password = password; // Ensure password is hashed
            }

            if (!string.IsNullOrEmpty(profilePhoto))
            {
                user.ProfilePhoto = profilePhoto;
            }

            if (dateOfBirth.HasValue)
            {
                user.DateOfBirth = dateOfBirth.Value;
            }

            if (!string.IsNullOrEmpty(address))
            {
                try
                {
                    var (latitude, longitude) = await GetCoordinatesAsync(address);
                    user.Location = new Location
                    {
                        Latitude = latitude,
                        Longitude = longitude,
                        Address = address,
                        PlaceId = "" // Optional: fetch and store PlaceId if needed
                    };
                }
                catch (Exception)
                {
                    return new ServiceResult { Success = false, Message = "Failed to fetch coordinates for the provided address" };
                }
            }

            await _userRepository.UpdateAsync(user);

            return new ServiceResult { Success = true, Message = "User details updated successfully" };
        }

        //TODO: Implement this method
        // write method that update string bio in user         
        public async Task<ServiceResult> UpdateUserDetailsAsync(int userId, string bio)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }
            // Update user properties if new values are provided
            if (!string.IsNullOrEmpty(bio))
            {
            //    user.Bio = bio;
            }
            await _userRepository.UpdateAsync(user);
            return new ServiceResult { Success = true, Message = "User details updated successfully" };
        } 
        private async Task<(double latitude, double longitude)> GetCoordinatesAsync(string address)
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

                double latitude = double.Parse(lat.Value);
                double longitude = double.Parse(lng.Value);

                return (latitude, longitude);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceResult> UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
            return new ServiceResult { Success = true, Message = "User updated successfully" };
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<ServiceResult> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            await _userRepository.RemoveAsync(user);
            return new ServiceResult { Success = true, Message = "User deleted successfully" };
        }

        //public async Task<IEnumerable<UserSubcategory>> GetSubcategoriesByUserIdAsync(int userId);
       

        public async Task<ServiceResult> AddUserSubcategoriesAsync(int userId, List<UserSubcategoryDto> subcategoryIds)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            foreach (var userSubcategoryDto in subcategoryIds)
            {
                var userSubcategory = new UserSubcategory
                {
                    UserId = userId,
                    SubcategoryId = userSubcategoryDto.SubcategoryId,
                    Weight = userSubcategoryDto.Weight
                };
                await _userSubcategoryRepository.AddAsync(userSubcategory);
            }
            
            return new ServiceResult { Success = true, Message = "User subcategories added successfully" };
        }

        public async Task<ServiceResult> RemoveUserSubcategoryAsync(int userId, int subcategoryId)
        {
            var userSubcategoryList = await _userSubcategoryRepository.FindAsync(us => us.UserId == userId && us.SubcategoryId == subcategoryId);
            var userSubcategory = userSubcategoryList.FirstOrDefault();

            if (userSubcategory == null)
            {
                return new ServiceResult { Success = false, Message = "User subcategory not found" };
            }

            await _userSubcategoryRepository.RemoveAsync(userSubcategory);
            return new ServiceResult { Success = true, Message = "User subcategory removed successfully" };
        }

        public async Task<ServiceResult> AddUserPreferredDestinationsAsync(int userId, List<UserPreferredDestination> preferredDestinations)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            foreach (var destination in preferredDestinations)
            {
                destination.UserId = userId;
                await _userPreferredDestinationRepository.AddAsync(destination);
            }

            return new ServiceResult { Success = true, Message = "User preferred destinations added successfully" };
        }

        public async Task<ServiceResult> AddUserAvailabilitiesAsync(int userId, List<UserAvailability> availabilities)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            foreach (var availability in availabilities)
            {
                availability.UserId = userId;
                await _userAvailabilityRepository.AddAsync(availability);
            }

            return new ServiceResult { Success = true, Message = "User availabilities added successfully" };
        }

        public Task<ServiceResult> UpdateUserDetailsAsync(int userId, string? name, string? email, string? password, string? profilePhoto, DateTime? dateOfBirth, Location? location)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> AddUserSubcategoriesAsync(int userId, List<int> subcategoryIds)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateUserDistanceWillingToTravelAsync(int userId, double distance)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            user.DistanceWillingToTravel = distance;
            await _userRepository.UpdateAsync(user);

            return new ServiceResult { Success = true, Message = "Distance willing to travel updated successfully" };
        }

        public Task<IEnumerable<UserSubcategory>> GetSubcategoriesByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
