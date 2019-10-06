using ChatOverflow.Attributes;
using ChatOverflow.Infrastructure.ChatProviders.GroupChatProvider;
using ChatOverflow.Infrastructure.SocketProvider;
using ChatOverflow.Models.DB.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.V1.Hubs
{
    [Authorize]
    [InjectableHub("v1/chat")]
    public class ChatHub : Hub
    {

        public static ConcurrentDictionary<string, List<string>> MyUsers = new ConcurrentDictionary<string, List<string>>();

        public const string GroupChatSymbol = "groupchat";
        public const string NewGroupMessageEvent = "NewGroupMessageEvent";

        private readonly ISocketProvider _socket;
        private readonly IGroupChatProvider _groupChat;

        public ChatHub(ISocketProvider socket, IGroupChatProvider groupChat)
        {
            _socket = socket;
            _groupChat = groupChat;
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {

            MyUsers.SingleOrDefault(x => x.Value.Contains(Context.ConnectionId)).Value.Remove(Context.ConnectionId);

            var emptyContainers = MyUsers.Where(x => x.Value.Count <= 0);
            foreach(var emptyContainer in emptyContainers)
            {
                MyUsers.Remove(emptyContainer.Key, out List<string> emptyList);
            }
        }


        public async Task StartAuth(string token)
        {
            var user = await _socket.GetUserByAccessToken(token);
            if(user == null)
            {
                // Send User the info that he isnt allowed
                Context.Abort();
                return;
            }

            var foundEntry = MyUsers.AddOrUpdate(user.Id, new List<string> { Context.ConnectionId }, (key, value) =>
            {
                value.Add(Context.ConnectionId);
                return value;
            });


            var groupChats = await _groupChat.GetByUserAsync(user);
            foreach(var groupChat in groupChats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "groupchat#" + groupChat.Id);
            }
        }

        public async Task SendGroupMessage(string groupId)
        {
            if (!MyUsers.SelectMany(x => x.Value).Contains(Context.ConnectionId))
                return;

            var group = Clients.Group(GroupChatSymbol + "#" + groupId);
            if (group == null)
                return;
            await group.SendAsync(NewGroupMessageEvent);
        }
    }
}
