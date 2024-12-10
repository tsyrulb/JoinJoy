﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Models
{
    public class SimpleMessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
