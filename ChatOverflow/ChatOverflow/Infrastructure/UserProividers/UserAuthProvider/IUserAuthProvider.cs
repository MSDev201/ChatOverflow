using ChatOverflow.Attributes;
using ChatOverflow.Models.DB.UserModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ChatOverflow.Infrastructure.UserProividers.UserAuthProvider
{
    [InjectableInterface]
    public interface IUserAuthProvider
    {
        Task<string> GenerateJWTokenFromUserAsync(string userId);
        Task<string> GenerateJWTokenFromUserAsync(User user, string oldToken = null);
        Task<SignInResult> SignInAsync(User user, string password, string twoFactorCode = null);
        Task<SignInResult> SignInAsync(string nameId, string password, string twoFactorCode = null);
    }
}
