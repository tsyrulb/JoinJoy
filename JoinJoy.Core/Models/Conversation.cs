using System;
using System.Collections.Generic;

namespace JoinJoy.Core.Models
{
    public class Conversation
    {
        public int Id { get; set; } // Unique identifier for the conversation
        public string? Title { get; set; } // Title for group chats (optional for one-on-one)

        // Navigation properties
        public ICollection<Message>? Messages { get; set; } // Messages in this conversation
        public ICollection<UserConversation> Participants { get; set; } = new List<UserConversation>(); // Users involved in the conversation
    }

    public class UserConversation
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }
}
