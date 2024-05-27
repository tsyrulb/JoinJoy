using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace JoinJoy.Core.Models
{
    public class Match
    {
        public int Id { get; set; } // Unique identifier for the match
        public int UserId1 { get; set; } // ID of the first user in the match
        public int UserId2 { get; set; } // ID of the second user in the match
        public int ActivityId { get; set; } // ID of the activity associated with the match
        public DateTime MatchDate { get; set; } // Date when the match was made
        public bool IsAccepted { get; set; } // Status indicating if the match was accepted

        // Navigation properties
        public User User1 { get; set; } // Reference to the first user
        public User User2 { get; set; } // Reference to the second user
        public Activity Activity { get; set; } // Reference to the activity
    }
}
