using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IConversationRepository _conversationRepository;
    private readonly ILogger<MessageService> _logger;
    private readonly INotificationService _notificationService;


    public MessageService(
        IMessageRepository messageRepository,
        IConversationRepository conversationRepository,
        ILogger<MessageService> logger,
        INotificationService notificationService)
    {
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<ServiceResult> SendMessageAsync(Message message)
    {
        try
        {
            if (string.IsNullOrEmpty(message.Content) || message.SenderId == 0)
            {
                return new ServiceResult { Success = false, Message = "Invalid message input." };
            }

            // Ensure there is at least one receiver or a conversation ID
            if (message.ConversationId == 0 && message.ReceiverId == 0)
            {
                return new ServiceResult { Success = false, Message = "Message must have a receiver or belong to a conversation." };
            }

            Conversation existingConversation;

            if (message.ConversationId > 0)
            {
                // If a conversation ID is provided, find the conversation
                existingConversation = await _conversationRepository.GetConversationWithMessagesAsync(message.ConversationId);
            }
            else
            {
                // Find an existing conversation between the sender and receiver
                existingConversation = await _conversationRepository.FindExistingConversationAsync(message.SenderId, message.ReceiverId);

                if (existingConversation == null)
                {
                    // Create a new conversation if one doesn't exist
                    existingConversation = new Conversation
                    {
                        Title = $"Conversation between {message.SenderId} and {message.ReceiverId}",
                        Participants = new List<UserConversation>
                        {
                            new UserConversation { UserId = message.SenderId },
                            new UserConversation { UserId = message.ReceiverId }
                        }
                    };

                    // Save the new conversation
                    await _conversationRepository.AddAsync(existingConversation);
                    await _conversationRepository.SaveChangesAsync();
                }
            }

            // Associate the message with the conversation
            message.ConversationId = existingConversation.Id;

            // Set additional message properties
            message.Timestamp = DateTime.UtcNow;
            message.IsRead = false;

            // Add and save the message
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();

            await _notificationService.NotifyConversationAsync(message.ConversationId, message);


            return new ServiceResult { Success = true, Message = "Message sent successfully" };
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An error occurred while sending a message.");

            // Return a failure result
            return new ServiceResult { Success = false, Message = "An error occurred while sending the message." };
        }
    }

    public async Task<ServiceResult> AddUsersToConversationAsync(int conversationId, List<int> userIds)
    {
        var conversation = await _conversationRepository.GetByIdAsync(conversationId);

        if (conversation == null)
        {
            return new ServiceResult { Success = false, Message = "Conversation not found." };
        }

        foreach (var userId in userIds)
        {
            // Check if the user is already in the conversation
            if (!conversation.Participants.Any(p => p.UserId == userId))
            {
                var userConversation = new UserConversation
                {
                    UserId = userId,
                    ConversationId = conversationId
                };
                conversation.Participants.Add(userConversation);
            }
        }

        await _conversationRepository.UpdateAsync(conversation);
        await _conversationRepository.SaveChangesAsync();

        return new ServiceResult { Success = true, Message = "Users added to the conversation successfully." };
    }

    public async Task<IEnumerable<Message>> GetMessagesForConversationAsync(int conversationId)
    {
        return await _messageRepository.GetMessagesForConversationAsync(conversationId);
    }

    public async Task<ServiceResult> MarkMessagesAsReadAsync(int conversationId, int userId)
    {
        var messages = await _messageRepository.FindAsync(m => m.ConversationId == conversationId && m.ReceiverId == userId && !m.IsRead);

        foreach (var message in messages)
        {
            message.IsRead = true;
            await _messageRepository.UpdateAsync(message);
        }

        return new ServiceResult { Success = true, Message = "All messages marked as read." };
    }

    public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId)
    {
        return await _conversationRepository.GetConversationsForUserAsync(userId);
    }

    public async Task<ServiceResult> DeleteMessageAsync(int messageId)
    {
        var message = await _messageRepository.GetByIdAsync(messageId);
        if (message == null)
        {
            return new ServiceResult { Success = false, Message = "Message not found" };
        }

        await _messageRepository.RemoveAsync(message);
        return new ServiceResult { Success = true, Message = "Message deleted successfully" };
    }

    public async Task<ServiceResult> DeleteConversationAsync(int conversationId)
    {
        var conversation = await _conversationRepository.GetConversationWithMessagesAsync(conversationId);
        if (conversation == null)
        {
            return new ServiceResult { Success = false, Message = "Conversation not found" };
        }

        await _conversationRepository.RemoveAsync(conversation);
        return new ServiceResult { Success = true, Message = "Conversation deleted successfully" };
    }
}
