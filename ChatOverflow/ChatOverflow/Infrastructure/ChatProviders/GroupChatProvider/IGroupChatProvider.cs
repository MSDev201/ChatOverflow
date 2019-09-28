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
        Task<GroupChat> AddMembersAsync(GroupChat chat, ICollection<User> members, bool areAdmins = false);
        Task<GroupChat> CreateAsync(string name, User creator, string password = null, bool createLink = false);
        Task<GroupChat> GetById(string id, User user = null);
        Task<bool> HasMember(GroupChat chat, User member);
    }
}
