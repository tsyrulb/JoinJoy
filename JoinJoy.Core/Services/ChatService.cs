using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IUserService _userService;
        private readonly IMatchService _matchService;

        public ChatService(IUserService userService, IMatchService matchService)
        {
            _userService = userService;
            _matchService = matchService;
        }

        public async Task<string> ProcessMessageAsync(ChatMessageRequest request)
        {
            if (request.Message.Contains("company"))
            {
                return "Hello, User! Do you want to have company in attending something?";
            }
            else if (request.Message.Contains("yes"))
            {
                return "What do you want to attend?";
            }
            else if (request.Message.Contains("museum"))
            {
                var matches = await _matchService.FindMatchesAsync(request.UserId, "museum");
                return $"I found a few users who also love visiting museums. Would you like to see their profiles?";
            }
            return "I'm not sure how to respond to that.";
        }
    }

}