using ChatOverflow.Attributes;
using ChatOverflow.Models.DB.ChatModels;
using ChatOverflow.Models.DB.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Infrastructure.ChatProviders.GroupChatProvider
{
    [InjectableInterface]
    public interface IGroupChatProvider
    {
        Task<GroupChat> AddMember(GroupChat chat, User member, bool isAdmin = false);
        Task<GroupChat> AddMembers(GroupChat chat, ICollection<User> members, bool areAdmins = false);
        Task<GroupChat> Create(string name, User creator, string password = null, bool createLink = false);
        Task<GroupChat> GetById(string id);
    }
}
