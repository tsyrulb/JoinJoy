using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JoinJoy.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // POST: api/messages/send
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            var result = await _messageService.SendMessageAsync(message);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/messages/conversation/{conversationId}
        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesForConversation(int conversationId)
        {
            var messages = await _messageService.GetMessagesForConversationAsync(conversationId);
            return Ok(messages);
        }

        // POST: api/messages/mark-read/{conversationId}/user/{userId}
        [HttpPost("mark-read/{conversationId}/user/{userId}")]
        public async Task<IActionResult> MarkMessagesAsRead(int conversationId, int userId)
        {
            var result = await _messageService.MarkMessagesAsReadAsync(conversationId, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/messages/conversations/user/{userId}
        [HttpGet("conversations/user/{userId}")]
        public async Task<IActionResult> GetConversationsForUser(int userId)
        {
            var conversations = await _messageService.GetConversationsForUserAsync(userId);
            return Ok(conversations);
        }

        // DELETE: api/messages/{messageId}
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var result = await _messageService.DeleteMessageAsync(messageId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // DELETE: api/messages/conversation/{conversationId}
        [HttpDelete("conversation/{conversationId}")]
        public async Task<IActionResult> DeleteConversation(int conversationId)
        {
            var result = await _messageService.DeleteConversationAsync(conversationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
