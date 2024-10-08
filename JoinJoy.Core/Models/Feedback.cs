using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinJoy.Core.Models
{
    public class Feedback
    {
        public int Id { get; set; } // Unique identifier for the feedback
        public int UserId { get; set; } // ID of the user providing the feedback
        public int ActivityId { get; set; } // ID of the activity being reviewed
        public int TargetUserId { get; set; } // ID of the user receiving the feedback

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } // Rating provided by the user
        public DateTime Timestamp { get; set; } // Time when the feedback was submitted

        // Navigation properties
        public User User { get; set; } // Reference to the user providing feedback
        public User TargetUser { get; set; } // Reference to the user receiving feedback
        public Activity Activity { get; set; } // Reference to the activity being reviewed
    }
    public class FeedbackRequest
    {
        public int UserId { get; set; } // ID of the user providing the feedback
        public int ActivityId { get; set; } // ID of the activity where feedback is being provided
        public int TargetUserId { get; set; } // ID of the user receiving the feedback
        public int Rating { get; set; } // Rating (1-5)
    }
}

