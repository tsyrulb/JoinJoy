using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using Microsoft.AspNetCore.SignalR;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyUserAsync(int userId, string message)
    {
        await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessage", message);
    }

    public async Task NotifyConversationAsync(int conversationId, Message message)
    {
        var dto = new SimpleMessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            Content = message.Content,
            Timestamp = message.Timestamp
        };

        await _hubContext.Clients.Group($"conversation_{conversationId}")
                                 .SendAsync("ReceiveMessage", dto);
    }

}
