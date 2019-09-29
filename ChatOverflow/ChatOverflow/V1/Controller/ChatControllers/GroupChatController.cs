using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatOverflow.Infrastructure.ChatProviders.GroupChatProvider;
using ChatOverflow.Infrastructure.UserProividers.UserProvider;
using ChatOverflow.Models.DB.ChatModels;
using ChatOverflow.Models.DB.UserModels;
using ChatOverflow.V1.Models.InputModels;
using ChatOverflow.V1.Models.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatOverflow.V1.Controller.ChatControllers
{
    [Authorize]
    public class GroupChatController : ApiController
    {

        private readonly IGroupChatProvider _groupChat;
        private readonly IUserProvider _user;

        public GroupChatController(IGroupChatProvider groupChat, IUserProvider user)
        {
            _groupChat = groupChat;
            _user = user;
        }

        #region Get

        [HttpGet("List/CurrentUser")]
        public async Task<IActionResult> GetAllByCurrentUser()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return Forbid();

            var groupChats = await _groupChat.GetByUserAsync(user);
            if (groupChats == null)
                return BadRequest();
            var res = new List<GroupChatResult>();
            foreach (var groupChat in groupChats)
                res.Add(new GroupChatResult(groupChat));
            return Ok(res);
        }

        [HttpGet("CurrentUser/{id}")]
        public async Task<IActionResult> GetByCurrentUser(string id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return Forbid();

            var groupChat = await _groupChat.GetByIdAsync(id, user);
            if (groupChat == null)
                return BadRequest();
            return Ok(new GroupChatResult(groupChat));
        }

        #endregion

        #region GetMessages

        [HttpGet("Messages/{groupId}")]
        public async Task<IActionResult> GetMessages(string groupId, int limit = 100)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return Forbid();
            var groupChat = await _groupChat.GetByIdAsync(groupId, user);
            if (groupChat == null)
                return NotFound();
            limit = limit > 1000 ? 1000 : limit;
            limit = limit < 0 ? 0 : limit;

            var messages = await _groupChat.GetMessagesAsync(groupChat, limit);
            var res = new List<ChatMessageResult>();

            foreach (var message in messages)
                res.Add(new ChatMessageResult(message));

            return Ok(res);
        }

        #endregion


        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateGroupChatInput groupIn)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return Forbid();

            var res = await _groupChat.CreateAsync(groupIn.Name, user, groupIn.Password, groupIn.CreateLink);
            if (res != null && res.Id != null)
            {
                // Add users
                var users = new List<User>();
                foreach(var memberId in groupIn.Members)
                {
                    var member = await _user.GetByIdAsync(memberId);
                    if (member == null)
                        continue;
                    users.Add(member);
                }
                await _groupChat.AddMembersAsync(res, users, false);

                return Ok(new GroupChatResult(res));
            }

            return BadRequest();
        }

        #endregion

        #region CreateMessage

        [HttpPost("Message/Create/{groupId}")]
        public async Task<IActionResult> CreateMessage(string groupId, [FromBody] ChatMessageInput inputMsg)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return Forbid();
            var groupChat = await _groupChat.GetByIdAsync(groupId, user);
            if (groupChat == null)
                return NotFound();

            var newMessage = await _groupChat.CreateMessageAsync(groupChat, user, inputMsg.Message);
            if (newMessage == null)
                return BadRequest();

            return Ok(new ChatMessageResult(newMessage));
        }

        #endregion
    }
}