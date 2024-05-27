using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesForUserAsync(int userId);
    }
}
