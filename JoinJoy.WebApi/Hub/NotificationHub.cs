using JoinJoy.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

[Authorize]
public class NotificationHub : Hub
{
    public async Task JoinConversationGroup(int conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
    }

    public async Task LeaveConversationGroup(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
    }

    public async Task SendMessageToConversation(int conversationId, SimpleMessageDto message)
    {
        // Broadcast this message to all clients in the specified conversation group
        var dto = new SimpleMessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            Content = message.Content,
            Timestamp = message.Timestamp
        };

        await Clients.Group($"conversation_{conversationId}").SendAsync("ReceiveMessage", dto);
    }
}
