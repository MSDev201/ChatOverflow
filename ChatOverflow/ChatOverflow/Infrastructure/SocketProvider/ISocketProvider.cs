using ChatOverflow.Attributes;
using ChatOverflow.Models.DB.UserModels;
using System.Threading.Tasks;

namespace ChatOverflow.Infrastructure.SocketProvider
{
    [InjectableInterface]
    public interface ISocketProvider
    {
        Task<UserSpecificToken> GenerateAccessTokenAsync(User user);
        Task<User> GetUserByAccessToken(string token);
    }
}