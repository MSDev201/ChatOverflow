using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Models.DB.ChatModels
{
    public class GroupChat
    {

        [MaxLength(128)]
        public string Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }

        #region Access

        public bool PublicAccess { get; set; }

        public bool LinkAccess { get; set; }
        public bool PasswordAccess { get; set; }
        public bool UsersInList { get; set; }

        #endregion

        public ICollection<GroupChatMember> Members { get; set; }
        public ICollection<ChatMessage> Messages { get; set; }

        public GroupChat()
        {
            PublicAccess = false;

            LinkAccess = false;
            PasswordAccess = false;
            UsersInList = true;

            Members = new List<GroupChatMember>();
            Messages = new List<ChatMessage>();
        }

    }
}
