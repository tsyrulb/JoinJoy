using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Services
{
    public interface IMessageService
    {
        Task<ServiceResult> SendMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync(int userId);
    }
}
