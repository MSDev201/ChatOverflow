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
        Task<ChatMessage> CreateMessageAsync(GroupChat chat, User sender, string message);
        Task<GroupChat> GetByIdAsync(string id, User user = null);
        Task<ICollection<GroupChat>> GetByUserAsync(User user);
        Task<ChatMessage> GetMessageByIdAsync(GroupChat chat, string msgId);
        Task<ICollection<ChatMessage>> GetMessagesAsync(GroupChat chat, int limit = 100);
        Task<ICollection<ChatMessage>> GetMessagesNewerThanAsync(GroupChat chat, string lastMessageId, int limit = 100);
        Task<ICollection<ChatMessage>> GetMessagesOlderThan(GroupChat chat, string lastMessageId, int limit = 100);
        Task<bool> HasMember(GroupChat chat, User member);
    }
}
