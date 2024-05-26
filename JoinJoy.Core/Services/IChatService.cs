using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    public interface IChatService
    {
        Task<string> ProcessMessageAsync(ChatMessageRequest request);
    }
}
