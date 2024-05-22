using JoinJoy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync(int userId);
        // Additional service methods
    }
}
