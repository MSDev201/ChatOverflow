using ChatOverflow.Models.DB.ChatModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Models.ResultModels
{
    public class ChatMessageResult
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDetailsResult CreatedBy { get; set; }

        public ChatMessageResult()
        {

        }

        public ChatMessageResult(ChatMessage msg)
        {
            Id = msg.Id;
            Message = msg.Message;
            CreatedAt = msg.CreatedAt;
            CreatedBy = new UserDetailsResult(msg.SentBy);
        }
    }
}
