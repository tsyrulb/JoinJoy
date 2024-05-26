using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Activity> Activities { get; }
        IRepository<Match> Matches { get; }
        IRepository<Message> Messages { get; }
        IRepository<Feedback> Feedbacks { get; }
        IRepository<ChatMessage> ChatMessages { get; }
        Task<int> CompleteAsync();
    }
}
