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

        public async Task<GroupChat> GetByIdAsync(string id, User user = null)
        {
            return await _context.GroupChats
                .SingleOrDefaultAsync(x => x.Id.Equals(id, StringComparison.Ordinal)
                && (user == null || user != null && x.Members.SingleOrDefault(y => y.Member.Id.Equals(user.Id, StringComparison.Ordinal)) != null));
        }

        public async Task<ICollection<GroupChat>> GetByUserAsync(User user)
        {
            return await _context.Entry(user).Collection(x => x.GroupChats).Query().Select(x => x.Chat).ToListAsync();
        }


        public async Task<ChatMessage> GetMessageByIdAsync(GroupChat chat, string msgId)
        {
            return await _context
                .Entry(chat)
                .Collection(x => x.Messages)
                .Query()
                .Include(x => x.SentBy)
                .SingleOrDefaultAsync(x => x.Id.Equals(msgId, StringComparison.Ordinal));
        }

        public async Task<ICollection<ChatMessage>> GetMessagesAsync(GroupChat chat, int limit = 100)
        {
            return await _context.Entry(chat)
                .Collection(x => x.Messages)
                .Query()
                .Include(x => x.SentBy)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<ICollection<ChatMessage>> GetMessagesOlderThan(GroupChat chat, string lastMessageId, int limit = 100)
        {
            var message = await GetMessageByIdAsync(chat, lastMessageId);
            if (message == null)
                return null;
            return await _context.Entry(chat)
                .Collection(x => x.Messages)
                .Query()
                .Include(x => x.SentBy)
                .Where(x => x.CreatedAt <= message.CreatedAt && x.Id != message.Id)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                //.OrderBy(x => x.CreatedAt)
                //.TakeWhile(x => !x.Id.Equals(message.Id, StringComparison.Ordinal))
                .ToListAsync();
        }

        public async Task<ICollection<ChatMessage>> GetMessagesNewerThanAsync(GroupChat chat, string lastMessageId, int limit = 100)
        {
            var message = await GetMessageByIdAsync(chat, lastMessageId);
            if (message == null)
                return null;
            return await _context.Entry(chat)
                .Collection(x => x.Messages)
                .Query()
                .Include(x => x.SentBy)
                .Where(x => x.CreatedAt >= message.CreatedAt && x.Id != message.Id)
                .OrderBy(x => x.CreatedAt)
                .Take(limit)
                //.OrderByDescending(x => x.CreatedAt)
                //.TakeWhile(x => !x.Id.Equals(message.Id, StringComparison.Ordinal))
                .ToListAsync();
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

        public async Task<ChatMessage> CreateMessageAsync(GroupChat chat, User sender, string message)
        {
            // TODO: Implement message limit in timeframe
            var newMessage = new ChatMessage
            {
                Message = message,
                SentBy = sender,
                CreatedAt = DateTime.Now
            };
            chat.Messages.Add(newMessage);
            await _context.SaveChangesAsync();
            return newMessage;
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
