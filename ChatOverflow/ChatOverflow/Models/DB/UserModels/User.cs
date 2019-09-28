using ChatOverflow.Models.DB.ChatModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Models.DB.UserModels
{
    public class User : IdentityUser
    {
        public ICollection<UserSpecificToken> Tokens { get; set; }

        public ICollection<GroupChatMember> GroupChats { get; set; }

        public User()
        {
            Tokens = new List<UserSpecificToken>();
            GroupChats = new List<GroupChatMember>();
        }
    }
}
