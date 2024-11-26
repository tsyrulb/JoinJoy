using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;

namespace JoinJoy.Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserActivityRepository _userActivityRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserActivityRepository userActivityRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userActivityRepository = userActivityRepository;
        }
        public async Task<ServiceResult> SubmitFeedbackAsync(Feedback feedback)
        {
            // Rule 1: Prevent users from submitting feedback to themselves
            if (feedback.UserId == feedback.TargetUserId)
            {
                return new ServiceResult { Success = false, Message = "You cannot submit feedback to yourself." };
            }

            // Rule 2: Check if feedback for the same user on the same activity already exists
            var existingFeedback = await _feedbackRepository.FindAsync(f =>
                f.UserId == feedback.UserId && f.ActivityId == feedback.ActivityId && f.TargetUserId == feedback.TargetUserId);

            if (existingFeedback.Any())
            {
                var feedbackToUpdate = existingFeedback.First();

                // Update the existing feedback if there are changes
                if (feedbackToUpdate.Rating != feedback.Rating || feedbackToUpdate.Timestamp != feedback.Timestamp)
                {
                    feedbackToUpdate.Rating = feedback.Rating;
                    feedbackToUpdate.Timestamp = DateTime.UtcNow;

                    await _feedbackRepository.UpdateAsync(feedbackToUpdate);
                    return new ServiceResult { Success = true, Message = "Feedback updated successfully." };
                }

                return new ServiceResult { Success = true, Message = "No changes detected; feedback remains the same." };
            }

            // Ensure that both users participated in the activity
            var userActivities = await _userActivityRepository.GetUsersInActivityAsync(feedback.ActivityId);
            if (!userActivities.Contains(feedback.UserId) || !userActivities.Contains(feedback.TargetUserId))
            {
                return new ServiceResult { Success = false, Message = "Both users must have participated in the same activity." };
            }

            // Proceed to add new feedback
            feedback.Timestamp = DateTime.UtcNow;
            await _feedbackRepository.AddAsync(feedback);

            return new ServiceResult { Success = true, Message = "Feedback submitted successfully." };
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackForActivityAsync(int activityId)
        {
            return await _feedbackRepository.GetFeedbackForActivityAsync(activityId);
        }

        public async Task<IEnumerable<Feedback>> GetUserFeedbackAsync(int userId)
        {
            return await _feedbackRepository.GetUserFeedbackAsync(userId);
        }

        public async Task<ServiceResult> UpdateFeedbackAsync(int id, FeedbackRequest feedbackRequest)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                return new ServiceResult { Success = false, Message = "Feedback not found." };
            }

            // Ensure the user who created the feedback is the one updating it
            if (feedback.UserId != feedbackRequest.UserId)
            {
                return new ServiceResult { Success = false, Message = "You are not authorized to update this feedback." };
            }

            // Update feedback details
            feedback.Rating = feedbackRequest.Rating;
            feedback.Timestamp = DateTime.UtcNow;

            await _feedbackRepository.UpdateAsync(feedback);

            return new ServiceResult { Success = true, Message = "Feedback updated successfully." };
        }

        public async Task<Feedback> GetFeedbackAsync(int id)
        {
            return await _feedbackRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> DeleteFeedbackAsync(int id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                return new ServiceResult { Success = false, Message = "Feedback not found." };
            }

            await _feedbackRepository.RemoveAsync(feedback);
            return new ServiceResult { Success = true, Message = "Feedback deleted successfully." };
        }
    }
}
