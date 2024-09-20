using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using JoinJoy.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IConversationRepository _conversationRepository;

    public MessageService(IMessageRepository messageRepository, IConversationRepository conversationRepository)
    {
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
    }

    // Send a new messages
    public async Task<ServiceResult> SendMessageAsync(Message message)
    {
        if (string.IsNullOrEmpty(message.Content) || message.SenderId == 0 || message.ReceiverId == 0)
        {
            return new ServiceResult { Success = false, Message = "Invalid message input." };
        }

        // Check if a conversation between the sender and receiver already exists
        var existingConversation = await _conversationRepository.FindExistingConversationAsync(message.SenderId, message.ReceiverId);

        if (existingConversation != null)
        {
            // Continue with the existing conversation
            message.ConversationId = existingConversation.Id;
        }
        else
        {
            // Create a new conversation if one doesn't exist
            var newConversation = new Conversation
            {
                // Optionally set properties like title, created date, etc.
                Title = $"Conversation between {message.SenderId} and {message.ReceiverId}",  // Set a default title
                Participants = new List<UserConversation>
            {
                new UserConversation { UserId = message.SenderId },
                new UserConversation { UserId = message.ReceiverId }
            }
            };

            // Save the new conversation to the database
            await _conversationRepository.AddAsync(newConversation);
            await _conversationRepository.SaveChangesAsync();

            // Assign the new conversation ID to the message
            message.ConversationId = newConversation.Id;
        }

        // Set the other message properties
        message.Timestamp = DateTime.UtcNow;
        message.IsRead = false;

        // Add and save the message
        await _messageRepository.AddAsync(message);
        await _messageRepository.SaveChangesAsync();

        return new ServiceResult { Success = true, Message = "Message sent successfully" };
    }



    // Retrieve messages from a conversation
    public async Task<IEnumerable<Message>> GetMessagesForConversationAsync(int conversationId)
    {
        return await _messageRepository.GetMessagesForConversationAsync(conversationId);
    }

    // Mark all messages in a conversation as read by a specific user
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

    // Retrieve all conversations for a user
    public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId)
    {
        return await _conversationRepository.GetConversationsForUserAsync(userId);
    }

    // Delete a specific message by ID
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

    // Delete a conversation and all its associated messages
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
