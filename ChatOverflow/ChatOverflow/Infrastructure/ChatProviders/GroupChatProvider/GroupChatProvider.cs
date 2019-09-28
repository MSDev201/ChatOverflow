using ChatOverflow.Attributes;
using ChatOverflow.Models.DB.ChatModels;
using ChatOverflow.Models.DB.UserModels;
using ChatOverflow.Persistent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Infrastructure.ChatProviders.GroupChatProvider
{
    [InjectableProvider]
    public class GroupChatProvider : IGroupChatProvider
    {

        private readonly CoreDbContext _context;

        public GroupChatProvider(CoreDbContext context)
        {
            _context = context;
        }

        #region Get

        public async Task<GroupChat> GetById(string id, User user = null)
        {
            return await _context.GroupChats
                .SingleOrDefaultAsync(x => x.Id.Equals(id, StringComparison.Ordinal)
                && (user == null || user != null && x.Members.SingleOrDefault(y => y.Member.Id.Equals(user.Id, StringComparison.Ordinal)) != null));
        }

        #endregion

        #region Create

        public async Task<GroupChat> CreateAsync(string name, User creator, string password = null, bool createLink = false)
        {
            var newChat = new GroupChat
            {
                Name = name,
                Password = password,
            };

            newChat.Members.Add(new GroupChatMember
            {
                Member = creator,
                IsAdmin = true,
            });

            // TODO: Generate link if wanted

            await _context.GroupChats.AddAsync(newChat);
            await _context.SaveChangesAsync();

            return newChat;
        }

        #endregion

        #region Check

        public async Task<bool> HasMember(GroupChat chat, User member)
        {
            var membersEntry = _context.Entry<GroupChat>(chat).Collection(x => x.Members);
            return (await membersEntry.Query().SingleOrDefaultAsync(x => x.Member.Id.Equals(member.Id, StringComparison.Ordinal))) != null;
        }

        #endregion

        #region Change

        public async Task<GroupChat> AddMember(GroupChat chat, User member, bool isAdmin = false)
        {
            if (await HasMember(chat, member))
                return chat;

            chat.Members.Add(new GroupChatMember
            {
                IsAdmin = isAdmin,
                Member = member
            });

            await _context.SaveChangesAsync();

            return chat;
        }

        public async Task<GroupChat> AddMembersAsync(GroupChat chat, ICollection<User> members, bool areAdmins = false)
        {
            foreach(var user in members)
            {
                if (await HasMember(chat, user))
                    continue;

                chat.Members.Add(new GroupChatMember
                {
                    IsAdmin = areAdmins,
                    Member = user
                });
            }

            await _context.SaveChangesAsync();

            return chat;
        }

        #endregion

    }
}
