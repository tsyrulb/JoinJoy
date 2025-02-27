﻿using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using Microsoft.IdentityModel.Tokens;
using DayOfWeek = JoinJoy.Core.Models.DayOfWeek;

namespace JoinJoy.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserSubcategory> _userSubcategoryRepository;
        private readonly string _googleApiKey;
        private readonly string _jwtSecret;
        private readonly ILocationRepository _locationRepository;
        private readonly BlobStorageService _blobStorageService;
        public UserService(
            IRepository<User> userRepository,
            IRepository<UserSubcategory> userSubcategoryRepository,
            string googleApiKey,
            string jwtSecret,
            ILocationRepository locationRepository,
            BlobStorageService blobStorageService
            )
        {
            _userRepository = userRepository;
            _userSubcategoryRepository = userSubcategoryRepository;
            _googleApiKey = googleApiKey ?? throw new ArgumentNullException(nameof(googleApiKey));
            _jwtSecret = jwtSecret;
            _locationRepository = locationRepository;
            _blobStorageService = blobStorageService;
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

            // Generate a JWT token (or another type of token) for the authenticated user
            var token = GenerateJwtToken(user);

            Console.WriteLine("User logged in successfully");
            return new ServiceResult { Success = true, Message = "User logged in successfully", Token = token };
        }

        // This is an example method for generating a JWT token
        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ServiceResult<string>> UploadUserProfilePhotoAsync(int userId, Stream photoStream, string fileName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult<string> { Success = false, Message = "User not found" };
            }

            var photoUrl = await _blobStorageService.UploadProfilePhotoAsync(photoStream, fileName);
            user.ProfilePhoto = photoUrl;
            await _userRepository.UpdateAsync(user);

            return new ServiceResult<string> { Success = true, Message = "Profile photo uploaded successfully", Data = photoUrl };
        }


        public async Task<ServiceResult> DeleteUserProfilePhotoAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.ProfilePhoto))
            {
                return new ServiceResult { Success = false, Message = "User or profile photo not found" };
            }

            var fileName = Path.GetFileName(user.ProfilePhoto);
            await _blobStorageService.DeleteProfilePhotoAsync(fileName);

            user.ProfilePhoto = null;
            await _userRepository.UpdateAsync(user);

            return new ServiceResult { Success = true, Message = "Profile photo deleted successfully" };
        }

        public async Task<ServiceResult> UpdateUserDetailsAsync(
    int userId,
    string? name,
    string? email,
    string? password,
    string? profilePhoto,
    DateTime? dateOfBirth,
    string? address,
    string? gender,
    double? distanceWillingToTravel
)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            if (!string.IsNullOrEmpty(name)) user.Name = name;
            if (!string.IsNullOrEmpty(email)) user.Email = email;
            if (!string.IsNullOrEmpty(password)) user.Password = password; // Consider hashing the password
            if (!string.IsNullOrEmpty(profilePhoto)) user.ProfilePhoto = profilePhoto;
            if (dateOfBirth.HasValue) user.DateOfBirth = dateOfBirth.Value;

            // Include gender validation
            var validGenders = new List<string> { "Male", "Female", "Other" };
            if (!string.IsNullOrEmpty(gender) && validGenders.Contains(gender))
            {
                user.Gender = gender;
            }

            // Update Distance Willing to Travel if provided
            if (distanceWillingToTravel.HasValue)
            {
                user.DistanceWillingToTravel = distanceWillingToTravel.Value;
            }

            // Update location based on address if provided
            if (!string.IsNullOrEmpty(address))
            {
                try
                {
                    var (latitude, longitude) = await GetCoordinatesAsync(address);

                    // Check if the location already exists
                    var existingLocation = await _locationRepository.FindAsync(
                        loc => loc.Latitude == latitude && loc.Longitude == longitude
                    );

                    if (existingLocation.Any())
                    {
                        // Use existing location
                        user.LocationId = existingLocation.First().Id;
                    }
                    else
                    {
                        // Create new location if it does not exist
                        var newLocation = new Location
                        {
                            Latitude = latitude,
                            Longitude = longitude,
                            Address = address
                        };

                        await _locationRepository.AddAsync(newLocation);
                        await _locationRepository.SaveChangesAsync();

                        user.LocationId = newLocation.Id;
                    }
                }
                catch (Exception ex)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = $"Failed to fetch coordinates for the provided address: {ex.Message}"
                    };
                }
            }

            await _userRepository.UpdateAsync(user);

            return new ServiceResult { Success = true, Message = "User details updated successfully" };
        }

        public async Task<Location?> GetUserLocationAsync(int userId)
        {
            // Fetch the user to get the LocationId
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.LocationId == null)  // Ensure LocationId is not null
            {
                return null;
            }

            // Use the LocationId to fetch the Location from the location repository
            var location = await _locationRepository.GetByIdAsync(user.LocationId);
            return location;
        }

        // Granular update function for email
        public async Task<ServiceResult> UpdateUserEmailAsync(int userId, string email)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return new ServiceResult { Success = false, Message = "User not found" };

            user.Email = email;
            await _userRepository.UpdateAsync(user);
            return new ServiceResult { Success = true, Message = "Email updated successfully" };
        }
        public async Task<ServiceResult> UpdateUserGenderAsync(int userId, string gender)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            // Assuming you want to restrict gender to "Male", "Female", or "Other"
            var validGenders = new List<string> { "Male", "Female", "Other" };
            if (!validGenders.Contains(gender))
            {
                return new ServiceResult { Success = false, Message = "Invalid gender value" };
            }

            user.Gender = gender;
            await _userRepository.UpdateAsync(user);
            return new ServiceResult { Success = true, Message = "Gender updated successfully" };
        }

        // Granular update function for password
        public async Task<ServiceResult> UpdateUserPasswordAsync(int userId, string password)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return new ServiceResult { Success = false, Message = "User not found" };

            user.Password = password; // Consider hashing the password
            await _userRepository.UpdateAsync(user);
            return new ServiceResult { Success = true, Message = "Password updated successfully" };
        }

        // Granular update function for profile photo
        public async Task<ServiceResult> UpdateUserProfilePhotoAsync(int userId, string profilePhoto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return new ServiceResult { Success = false, Message = "User not found" };

            user.ProfilePhoto = profilePhoto;
            await _userRepository.UpdateAsync(user);
            return new ServiceResult { Success = true, Message = "Profile photo updated successfully" };
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
        public async Task<(double latitude, double longitude)> GetCoordinatesAsync(string address)
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

        public async Task<ServiceResult> AddUserSubcategoriesAsync(int userId, List<int> subcategoryIds)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            foreach (var subcategoryId in subcategoryIds)
            {
                var userSubcategory = new UserSubcategory
                {
                    UserId = userId,
                    SubcategoryId = subcategoryId,
                    Weight = 1  // Default weight; modify as necessary
                };
                await _userSubcategoryRepository.AddAsync(userSubcategory);
            }

            return new ServiceResult { Success = true, Message = "User subcategories added successfully" };
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
        public async Task<int> GetOrCreateLocationAsync(string address, double latitude, double longitude)
        {
            // Check if location already exists
            var existingLocations = await _locationRepository.FindAsync(
                loc => loc.Latitude == latitude && loc.Longitude == longitude);

            if (existingLocations.Any())
            {
                return existingLocations.First().Id;
            }
            else
            {
                var newLocation = new Location
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Address = address
                };

                await _locationRepository.AddAsync(newLocation);
                await _locationRepository.SaveChangesAsync();
                return newLocation.Id;
            }
        }




        public async Task<IEnumerable<UserSubcategory>> GetSubcategoriesByUserIdAsync(int userId)
        {
            var userSubcategories = await _userSubcategoryRepository.FindAsync(us => us.UserId == userId);
            return userSubcategories ?? new List<UserSubcategory>();
        }

    }
}
