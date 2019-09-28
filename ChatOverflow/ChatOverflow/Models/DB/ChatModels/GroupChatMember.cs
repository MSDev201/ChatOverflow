using ChatOverflow.Models.DB.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Models.DB.ChatModels
{
    
    public class GroupChatMember
    {
        [MaxLength(128)]
        public string Id { get; set; }

        public User Member { get; set; }
        public bool IsAdmin { get; set; }

        public DateTime EnteredAt { get; set; }

        public GroupChat Chat { get; set; }

    }
}
