using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JoinJoy.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Activity> _activityRepository;
        private readonly IFeedbackRepository _feedbackRepository;

        public AdminService(IUserRepository userRepository, IRepository<Activity> activityRepository, IFeedbackRepository feedbackRepository)
        {
            _userRepository = userRepository;
            _activityRepository = activityRepository;
            _feedbackRepository = feedbackRepository;
        }

        // Promote a user to admin
        public async Task<ServiceResult> PromoteUserToAdminAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            user.IsAdmin = true;
            await _userRepository.UpdateAsync(user);

            return new ServiceResult { Success = true, Message = $"User {user.Name} promoted to admin successfully" };
        }

        // Deactivate a user
        public async Task<ServiceResult> DeactivateUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult { Success = false, Message = "User not found" };
            }

            user.IsAdmin = false;
            await _userRepository.UpdateAsync(user);

            return new ServiceResult { Success = true, Message = $"User {user.Name} deactivated successfully" };
        }

        // Delete an activity
        public async Task<ServiceResult> DeleteActivityAsync(int activityId)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null)
            {
                return new ServiceResult { Success = false, Message = "Activity not found" };
            }

            await _activityRepository.RemoveAsync(activity);

            return new ServiceResult { Success = true, Message = "Activity deleted successfully" };
        }

        // Delete inappropriate feedback
        public async Task<ServiceResult> DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback == null)
            {
                return new ServiceResult { Success = false, Message = "Feedback not found" };
            }

            await _feedbackRepository.RemoveAsync(feedback);

            return new ServiceResult { Success = true, Message = "Feedback deleted successfully" };
        }

        // List all users
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        // List all activities
        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _activityRepository.GetAllAsync();
        }

        // List all feedbacks
        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _feedbackRepository.GetAllAsync();
        }
    }
}
