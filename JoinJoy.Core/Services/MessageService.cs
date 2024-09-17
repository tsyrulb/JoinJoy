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

    // Send a new message
    public async Task<ServiceResult> SendMessageAsync(Message message)
    {
        if (string.IsNullOrEmpty(message.Content) || message.SenderId == 0 || message.ConversationId == 0)
        {
            return new ServiceResult { Success = false, Message = "Invalid message input." };
        }

        message.Timestamp = DateTime.UtcNow;
        message.IsRead = false;

        await _messageRepository.AddAsync(message);

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
