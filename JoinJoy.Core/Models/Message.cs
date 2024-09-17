using JoinJoy.Core.Models;

public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }  // ReceiverId can be null if it's a group chat.
    public int ConversationId { get; set; } // Ties the message to a conversation
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }

    // Navigation properties
    public User Sender { get; set; }
    public User Receiver { get; set; }
    public Conversation Conversation { get; set; }
}
