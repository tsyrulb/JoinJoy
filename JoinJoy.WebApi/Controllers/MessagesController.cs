using Microsoft.AspNetCore.Mvc;
using JoinJoy.Core.Services;
using JoinJoy.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JoinJoy.WebApi.Controllers
{
    [Authorize] // Require authentication for all actions in this controller
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest messageRequest)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized("User ID is missing or invalid in token.");
            }

            if (messageRequest.ReceiverIds == null || !messageRequest.ReceiverIds.Any())
            {
                return BadRequest("Receiver ID is required.");
            }

            if (string.IsNullOrEmpty(messageRequest.Content))
            {
                return BadRequest("Message content is required.");
            }

            // Use only the first receiver from the list
            int receiverId = messageRequest.ReceiverIds.First();

            // Set the sender ID from the token
            messageRequest.SenderId = userId;

            // Create the message object
            var message = new Message
            {
                SenderId = messageRequest.SenderId,
                ReceiverId = receiverId,
                ConversationId = messageRequest.ConversationId,
                Content = messageRequest.Content,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            // Send the message
            var result = await _messageService.SendMessageAsync(message);

            if (result.Success)
            {
                return Ok(new { Success = true, Message = "Message sent successfully" });
            }

            return BadRequest(result);
        }



        // GET: api/messages/conversation/{conversationId}
        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesForConversation(int conversationId)
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var messages = await _messageService.GetMessagesForConversationAsync(conversationId);
                return Ok(messages);
            }
            return Unauthorized("User ID is missing or invalid in token.");
        }

        // POST: api/messages/mark-read/{conversationId}
        [HttpPost("mark-read/{conversationId}")]
        public async Task<IActionResult> MarkMessagesAsRead(int conversationId)
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var result = await _messageService.MarkMessagesAsReadAsync(conversationId, userId);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return Unauthorized("User ID is missing or invalid in token.");
        }

        [HttpPost("add-users-to-conversation")]
        public async Task<IActionResult> AddUsersToConversation(int conversationId, [FromBody] List<int> userIds)
        {
            var result = await _messageService.AddUsersToConversationAsync(conversationId, userIds);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        // GET: api/messages/conversations
        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversationsForUser()
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var conversations = await _messageService.GetConversationsForUserAsync(userId);
                return Ok(conversations);
            }
            return Unauthorized("User ID is missing or invalid in token.");
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
