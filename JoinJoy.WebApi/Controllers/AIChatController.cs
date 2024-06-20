using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;

namespace JoinJoy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIChatController : ControllerBase
    {
        private readonly IAIChatService _aiChatService;

        public AIChatController(IAIChatService aiChatService)
        {
            _aiChatService = aiChatService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest chatRequest)
        {
            var response = await _aiChatService.GetChatResponseAsync(chatRequest.UserInput);
            return Ok(response);
        }
    }

    public class ChatRequest
    {
        public string UserInput { get; set; }
    }
}
