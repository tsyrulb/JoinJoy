using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinJoy.Core.Models
{
    public class Match
    {
        [Key]
        public int MatchID { get; set; }
        public int UserID1 { get; set; }
        public int UserID2 { get; set; }
        public int ActivityID { get; set; }
        public DateTime MatchDate { get; set; }

        [ForeignKey("UserID1")]
        public User User1 { get; set; }

        [ForeignKey("UserID2")]
        public User User2 { get; set; }

        [ForeignKey("ActivityID")]
        public Activity Activity { get; set; }
    }
}
