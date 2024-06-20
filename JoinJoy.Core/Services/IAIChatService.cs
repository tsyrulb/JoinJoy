using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IAIChatService
    {
        Task<string> GetChatResponseAsync(string userInput);
    }
}
