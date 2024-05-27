using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class UserActivity
    {
        public int UserId { get; set; } // ID of the user participating in the activity
        public User User { get; set; } // Reference to the user

        public int ActivityId { get; set; } // ID of the activity
        public Activity Activity { get; set; } // Reference to the activity
    }
}
