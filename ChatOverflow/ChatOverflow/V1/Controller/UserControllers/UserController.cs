using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatOverflow.Infrastructure.UserProividers.UserProvider;
using ChatOverflow.Models.DB.UserModels;
using ChatOverflow.Utils;
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

            return Ok(new UserDetailsResult(user));
        }

        [HttpGet("List/Search/{term}")]
        public async Task<IActionResult> GetListBySearchTerm(string term, bool includeOwnUser = false)
        {
            var users = await _user.GetAllMatchingTerm(term);
            if (users == null)
                return BadRequest();

            if (!includeOwnUser)
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized();
                var ownUser = await _user.GetByIdAsync(userId);
                if (ownUser == null)
                    return Forbid();
                if (users.Contains(ownUser))
                    users.Remove(ownUser);
            }

            var res = new List<UserDetailsResult>();
            foreach(var user in users)
            {
                res.Add(new UserDetailsResult(user));
            }
            return Ok(res.OrderBy(x => LevenshteinDistance.Compute(x.DisplayName, term)));
        }

        #endregion
    }
}