using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoinJoy.Core.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("SenderID")]
        public User Sender { get; set; }

        [ForeignKey("ReceiverID")]
        public User Receiver { get; set; }
    }
}
