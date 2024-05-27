using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JoinJoy.Core.Models
{
    public class ChatMessage
    {
        public int Id { get; set; } // Unique identifier for the message
        public int SenderId { get; set; } // ID of the user who sent the message
        public int ReceiverId { get; set; } // ID of the user who received the message
        public string Content { get; set; } // Content of the message
        public DateTime Timestamp { get; set; } // Time when the message was sent
        public bool IsRead { get; set; } // Flag indicating if the message has been read

        // Navigation properties
        public User Sender { get; set; } // Reference to the sender user
        public User Receiver { get; set; } // Reference to the receiver user
    }
}
