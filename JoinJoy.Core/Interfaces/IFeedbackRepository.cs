using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Interfaces
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetFeedbackForActivityAsync(int activityId);
        Task<IEnumerable<Feedback>> GetUserFeedbackAsync(int userId);
    }
}
