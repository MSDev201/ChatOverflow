using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatOverflow.Infrastructure.UserProividers.UserProvider;
using ChatOverflow.V1.Models.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatOverflow.V1.Controller.UserControllers
{
    [Authorize]
    public class UserController : ApiController
    {

        private readonly IUserProvider _user;

        public UserController(IUserProvider user)
        {
            _user = user;
        }

        #region Get

        [HttpGet("Details")]
        public async Task<IActionResult> GetDetails()
        {
            var userId = GetCurrentUserId();
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return Forbid();

            return Ok(new UserDetailsResult
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.UserName, // TODO: Let user chose the displayname
                Email = user.Email
            });
        }

        #endregion

    }
}