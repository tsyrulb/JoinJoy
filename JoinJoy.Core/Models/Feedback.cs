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
        [Key]
        public int FeedbackID { get; set; }
        public int UserID { get; set; }
        public int ActivityID { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime FeedbackDate { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        [ForeignKey("ActivityID")]
        public Activity Activity { get; set; }
    }
}
