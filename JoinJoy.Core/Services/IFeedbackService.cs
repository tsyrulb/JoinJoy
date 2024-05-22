using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    public interface IFeedbackService
    {
        Task SubmitFeedbackAsync(Feedback feedback);
        Task<IEnumerable<Feedback>> GetFeedbacksAsync(int userId);
        // Additional service methods
    }
}