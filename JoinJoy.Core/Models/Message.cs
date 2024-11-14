using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinJoy.Core.Models
{
    public class Message
    {
        public int Id { get; set; } // Unique identifier for the message

        [Required]
        public int SenderId { get; set; } // ID of the user who sent the message

        [Required]
        public int ReceiverId { get; set; } // ID of the user who received the message

        [Required]
        public int ConversationId { get; set; } // ID of the conversation

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } // Content of the message

        public DateTime Timestamp { get; set; } // Time when the message was sent
        public bool IsRead { get; set; } // Flag indicating if the message has been read

        [ForeignKey("SenderId")]
        [JsonIgnore]
        public User? Sender { get; set; } // Reference to the sender user

        [ForeignKey("ReceiverId")]
        [JsonIgnore]  // Ignore during validation
        public User? Receiver { get; set; } // Reference to the receiver user

        [ForeignKey("ConversationId")]
        [JsonIgnore]  // Ignore during validation
        public Conversation? Conversation { get; set; } // Reference to the conversation
    }

    public class MessageRequest
    {
        public int SenderId { get; set; }
        public int ConversationId { get; set; }
        public string Content { get; set; }
        public List<int> ReceiverIds { get; set; } = new List<int>();
    }

}
