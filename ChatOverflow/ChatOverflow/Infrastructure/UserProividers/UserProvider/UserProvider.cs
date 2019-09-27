using ChatOverflow.Attributes;
using ChatOverflow.Models.DB.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Infrastructure.UserProividers.UserProvider
{
    [InjectableProvider]
    public class UserProvider : IUserProvider
    {
        private readonly UserManager<User> _userManager;

        public UserProvider(
            UserManager<User> userManager
            )
        {
            _userManager = userManager;
        }

        #region GetBy

        public async Task<User> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            return user;
        }

        public async Task<User> GetByNameOrEmailAsync(string nameId)
        {
            var user = await _userManager.FindByEmailAsync(nameId);
            if (user == null)
                user = await _userManager.FindByNameAsync(nameId);
            if (user == null)
                return null;
            return user;
        }

        #endregion

        #region GetListBy

        public async Task<List<User>> GetAllMatchingTerm(string searchTerm)
        {
            return await _userManager.Users
                .Where(x => x.NormalizedUserName.Contains(searchTerm.Normalize()))
                .ToListAsync();
        }

        #endregion

        #region Create

        public async Task<IdentityResult> CreateAsync(string username, string email, string password)
        {
            var newUser = new User
            {
                UserName = username,
                Email = email
            };

            var creationResult = await _userManager.CreateAsync(newUser, password);

            return creationResult;
        }

        public async Task<IList<IdentityError>> CheckCreateableAsync(string username, string email, string password)
        {
            var errors = new List<IdentityError>();
            var newUser = new User
            {
                UserName = username,
                Email = email
            };

            // Password validation
            var passwordValids = _userManager.PasswordValidators;
            foreach (var passwordValidator in passwordValids)
            {
                var validRes = await passwordValidator.ValidateAsync(_userManager, newUser, password);
                if (!validRes.Succeeded)
                    errors.AddRange(validRes.Errors.ToList());
            }

            // User validation
            var userValids = _userManager.UserValidators;
            foreach (var userValidator in userValids)
            {
                var validRes = await userValidator.ValidateAsync(_userManager, newUser);
                if (!validRes.Succeeded)
                    errors.AddRange(validRes.Errors.ToList());
            }


            return errors;
        }

        #endregion
    }
}
