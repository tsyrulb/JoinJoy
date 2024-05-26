using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;

namespace JoinJoy.WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(int user, string message)
        {
            var response = await _chatService.ProcessMessageAsync(new ChatMessageRequest { UserId = user, Message = message });
            await Clients.All.SendAsync("ReceiveMessage", user, response);
        }
    }
}
