using ChatOverflow.Models.DB.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Models.DB.ChatModels
{
    public class ChatMessage
    {
        [MaxLength(128)]
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public User SentBy { get; set; }

        public GroupChat GroupChat { get; set; }

        public ChatMessage()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
