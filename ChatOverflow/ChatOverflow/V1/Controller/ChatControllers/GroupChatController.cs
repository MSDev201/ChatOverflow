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
            return Ok(GroupChatToResult(groupChats));
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

                return Ok(GroupChatToResult(res));
            }

            return BadRequest();
        }

        #endregion


        #region NonActions

        [NonAction]
        public ICollection<GroupChatResult> GroupChatToResult(ICollection<GroupChat> inputObj)
        {
            var chats = new List<GroupChatResult>();
            foreach(var inputChat in inputObj)
            {
                chats.Add(GroupChatToResult(inputChat));
            }
            return chats;
        }

        [NonAction]
        public GroupChatResult GroupChatToResult(GroupChat inputObj)
        {

            return new GroupChatResult
            {
                Id = inputObj.Id,
                Name = inputObj.Name,
            };
        }

        #endregion
    }
}