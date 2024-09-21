namespace JoinJoy.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfilePhoto { get; set; }
        public DateTime? DateOfBirth { get; set; } // User's date of birth
        public bool? IsAdmin { get; set; }
        public Location? Location { get; set; }
        public DayOfWeek? UnavailableDay { get; set; } // Nullable in case user has no unavailability
        public TimeSpan? UnavailableStartTime { get; set; } // Start of unavailability
        public TimeSpan? UnavailableEndTime { get; set; } // End of unavailability
        public ICollection<UserSubcategory>? UserSubcategories { get; set; } // Many-to-many relationship with subcategories
        public ICollection<UserPreferredDestination>? UserPreferredDestinations { get; set; }
        public double? DistanceWillingToTravel { get; set; }

        // Navigation properties
        public ICollection<UserActivity>? UserActivities { get; set; }
        public ICollection<Match>? Matches { get; set; }
        public ICollection<Message>? SentMessages { get; set; } // Sent messages
        public ICollection<Message>? ReceivedMessages { get; set; } // Received messages

        public ICollection<UserConversation>? UserConversations { get; set; } // Received messages

        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<Activity>? CreatedActivities { get; set; } // Added this line

        // Separate collections for ChatMessages if they are indeed different from Messages
        public ICollection<ChatMessage>? SentChatMessages { get; set; } // Sent chat messages
        public ICollection<ChatMessage>? ReceivedChatMessages { get; set; } // Received chat messages
    }
}
