using ChatOverflow.Models.DB.ChatModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Models.ResultModels
{
    public class GroupChatResult
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public GroupChatResult()
        {

        }

        public GroupChatResult(GroupChat chat)
        {
            Id = chat.Id;
            Name = chat.Name;
        }
    }
}
