using ChatOverflow.Attributes;
using ChatOverflow.Models.DB.UserModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Infrastructure.UserProividers.UserProvider
{
    [InjectableInterface]
    public interface IUserProvider
    {
        Task<IList<IdentityError>> CheckCreateableAsync(string username, string email, string password);
        Task<IdentityResult> CreateAsync(string username, string email, string password);
        Task<User> GetByIdAsync(string userId);
        Task<User> GetByNameOrEmailAsync(string nameId);
    }
}
