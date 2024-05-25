using JoinJoy.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("message")]
    public async Task<IActionResult> PostMessage(ChatMessageRequest request)
    {
        var response = await _chatService.ProcessMessageAsync(request);
        return Ok(response);
    }
}
