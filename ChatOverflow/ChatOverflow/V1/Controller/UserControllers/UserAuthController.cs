using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatOverflow.Infrastructure.UserProividers.UserAuthProvider;
using ChatOverflow.Infrastructure.UserProividers.UserProvider;
using ChatOverflow.V1.Models.ContentModels;
using ChatOverflow.V1.Models.InputModels;
using ChatOverflow.V1.Models.ResultModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatOverflow.V1.Controller.UserControllers
{
    public class UserAuthController : ApiController
    {

        private readonly IUserProvider _user;
        private readonly IUserAuthProvider _userAuth;

        public UserAuthController(
            IUserProvider user,
            IUserAuthProvider userAuth
            )
        {
            _user = user;
            _userAuth = userAuth;
        }

        #region SIGNUP

        [HttpPost("SignUp/Check")]
        public async Task<IActionResult> SignUpCheck([FromBody] UserSignUpInput signUpData)
        {
            if (signUpData == null)
                return BadRequest();

            var errorRes = new ErrorResult();

            var errors = await _user.CheckCreateableAsync(signUpData.UserName, signUpData.EMail, signUpData.Password);

            foreach (var error in errors)
            {
                errorRes.Errors.Add(new ErrorCodeResult
                {
                    Code = error.Code,
                    Description = error.Description,
                });
            }

            return Ok(errorRes);

        }

        [HttpPost("SignUp/Create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ErrorCodeResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorCodeResult))]
        public async Task<IActionResult> SignUpCreate([FromBody] UserSignUpInput signUpData)
        {
            if (signUpData == null)
                return BadRequest();

            var createRes = await _user.CreateAsync(signUpData.UserName, signUpData.EMail, signUpData.Password);

            if (createRes == null)
                return BadRequest();

            var errorRes = new ErrorResult();
            foreach (var error in createRes.Errors)
            {
                errorRes.Errors.Add(new ErrorCodeResult
                {
                    Code = error.Code,
                    Description = error.Description,
                });
            }
            if (!createRes.Succeeded)
                return BadRequest(errorRes);

            return Created("", errorRes);
        }

        #endregion

        #region SIGNIN

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInInput userSignIn)
        {
            // Get user
            var user = await _user.GetByNameOrEmailAsync(userSignIn.NameId);
            if (user == null)
                return BadRequest();

            if (string.IsNullOrEmpty(userSignIn.TwoFactorCode) && user.TwoFactorEnabled)
                return BadRequest("MISSING_2FA");

            // try sign in
            var signResult = await _userAuth.SignInAsync(user, userSignIn.Password, userSignIn.TwoFactorCode);
            if (signResult.Succeeded)
            {
                // Generatetoken
                var token = await _userAuth.GenerateJWTokenFromUserAsync(user);
                if (token == null)
                    return BadRequest();
                return Ok(new SignInSuccessResult
                {
                    Token = token,
                });
            }
            else if (signResult.IsNotAllowed || signResult.IsLockedOut)
            {
                return Forbid();
            }

            return BadRequest();
        }

        [HttpPut("Token/Refresh/{userId}")]
        public async Task<IActionResult> RefreshToken(string userId, [FromBody] StringContent oldToken)
        {

            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return BadRequest();

            if (string.IsNullOrEmpty(oldToken.Value))
                return BadRequest();

            var newToken = await _userAuth.GenerateJWTokenFromUserAsync(user, oldToken.Value);
            if (newToken == null)
                return BadRequest();
            return Ok(new StringContent { Value = newToken });

        }

        #endregion
    }
}