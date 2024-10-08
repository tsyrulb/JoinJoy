using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IActivityRepository Activities { get; }
        IFeedbackRepository Feedbacks { get; }
        IMatchRepository Matches { get; }
        IMessageRepository Messages { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
