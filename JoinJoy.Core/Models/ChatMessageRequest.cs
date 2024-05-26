using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JoinJoy.Core.Models
{
    public class ChatMessageRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
