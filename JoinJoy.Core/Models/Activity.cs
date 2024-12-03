using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JoinJoy.Core.Models
{
    public class Activity
    {
        public int Id { get; set; } // Unique identifier for the activity
        public string Name { get; set; } // Name of the activity
        public string? Description { get; set; } // Detailed description of the activity
        public DateTime Date { get; set; } // Date and time when the activity takes place
        public int LocationId { get; set; } // ID of the location

        public Location Location { get; set; } // Location of the activity
        public int CreatedById { get; set; } // ID of the user who created the activity
        public int ConversationId { get; set; } // ID of the conversation for the activity
        public Conversation Conversation { get; set; } // Associated conversation

        // Navigation properties
        public User CreatedBy { get; set; } // Reference to the user who created the activity
        public ICollection<UserActivity> UserActivities { get; set; } // Many-to-many relationship with users
        public ICollection<Match> Matches { get; set; } // Collection of matches for the activity
    }
}
