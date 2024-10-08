

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinJoy.Core.Models
{
    public class Feedback
    {
        public int Id { get; set; } // Unique identifier for the feedback
        public int UserId { get; set; } // ID of the user providing the feedback
        public int ActivityId { get; set; } // ID of the activity being reviewed
        public string Comments { get; set; } // Feedback comments from the user
        public int Rating { get; set; } // Rating provided by the user
        public DateTime Timestamp { get; set; } // Time when the feedback was submitted

        // Navigation properties
        public User User { get; set; } // Reference to the user providing feedback
        public Activity Activity { get; set; } // Reference to the activity being reviewed
    }
}
