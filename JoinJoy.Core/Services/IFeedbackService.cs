using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Services
{
    public interface IFeedbackService
    {
        Task<ServiceResult> SubmitFeedbackAsync(Feedback feedback);
        Task<IEnumerable<Feedback>> GetUserFeedbackAsync(int userId);
        Task<IEnumerable<Feedback>> GetFeedbackForActivityAsync(int activityId);
        Task<ServiceResult> DeleteFeedbackAsync(int id);
        Task<ServiceResult> UpdateFeedbackAsync(int id, FeedbackRequest feedbackRequest);
        Task<Feedback> GetFeedbackAsync(int id);
    }
}
