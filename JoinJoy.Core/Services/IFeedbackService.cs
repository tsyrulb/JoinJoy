using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Services
{
    public interface IFeedbackService
    {
        Task<ServiceResult> SubmitFeedbackAsync(Feedback feedback);
        Task<IEnumerable<Feedback>> GetFeedbackAsync(int userId);
        Task<IEnumerable<Feedback>> GetUserFeedbackAsync(int userId);
        Task<IEnumerable<Feedback>> GetFeedbackForActivityAsync(int activityId);
    }
}
