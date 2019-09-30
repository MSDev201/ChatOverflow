using ChatOverflow.Attributes;
using ChatOverflow.Infrastructure.UserProividers.UserProvider;
using ChatOverflow.Models.DB.UserModels;
using ChatOverflow.Persistent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatOverflow.Infrastructure.SocketProvider
{
    [InjectableProvider]
    public class SocketProvider : ISocketProvider
    {

        private readonly CoreDbContext _context;
        private readonly IUserProvider _user;

        public SocketProvider(CoreDbContext context, IUserProvider user)
        {
            _context = context;
            _user = user;
        }


        public async Task<UserSpecificToken> GenerateAccessTokenAsync(User user)
        {
            await CheckAndDeleteOldTokens();
            UserSpecificToken newToken = null;
            do
            {
                newToken = UserSpecificToken.Generate(UserSpecificTokenType.SocketToken, DateTime.Now.AddHours(12));
            } while (await GetUserByAccessToken(newToken.Token) != null);
            user.Tokens.Add(newToken);
            await _context.SaveChangesAsync();
            return newToken;
        }

        public async Task<User> GetUserByAccessToken(string token)
        {
            var res = await _context.UserSpecificTokens
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Type == UserSpecificTokenType.SocketToken && x.Token.Equals(token, StringComparison.Ordinal));
            if (res == null)
                return null;
            return res.User;
        }

        private async Task CheckAndDeleteOldTokens()
        {
            var deleteTokens = _context.UserSpecificTokens.Where(x => !x.Active && x.Type == UserSpecificTokenType.SocketToken);
            _context.UserSpecificTokens.RemoveRange(deleteTokens);
            await _context.SaveChangesAsync();
        }

    }
}
