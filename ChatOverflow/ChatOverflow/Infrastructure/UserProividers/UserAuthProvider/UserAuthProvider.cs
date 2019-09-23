using ChatOverflow.Attributes;
using ChatOverflow.Infrastructure.UserProividers.UserProvider;
using ChatOverflow.Models.DB.UserModels;
using ChatOverflow.Persistent;
using ChatOverflow.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatOverflow.Infrastructure.UserProividers.UserAuthProvider
{
    [InjectableProvider]
    public class UserAuthProvider : IUserAuthProvider
    {

        private readonly IConfiguration _config;
        private readonly IUserProvider _user;
        private readonly SignInManager<User> _signInManager;
        private readonly CoreDbContext _context;

        public UserAuthProvider(
            SignInManager<User> signInManager,
            IConfiguration config,
            IUserProvider user,
            CoreDbContext context
            )
        {
            _config = config;
            _user = user;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<SignInResult> SignInAsync(User user, string password, string twoFactorCode = null)
        {
            if (user.TwoFactorEnabled)
            {
                if (twoFactorCode == null)
                    return null;
                // Check credentials
                var signRes = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (signRes == null)
                    return null;
                // Check 2fa
                var twoFARes = await _signInManager.UserManager.VerifyTwoFactorTokenAsync(user, _signInManager.Options.Tokens.AuthenticatorTokenProvider, twoFactorCode);
                if (twoFARes)
                    return signRes;
                return null;
            }

            var normalSignRes = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (normalSignRes == null)
                return null;
            return normalSignRes;
        }

        public async Task<SignInResult> SignInAsync(string nameId, string password, string twoFactorCode = null)
        {
            var user = await _user.GetByNameOrEmailAsync(nameId);
            if (user == null)
                return null;

            return await SignInAsync(user, password, twoFactorCode);
        }

        public async Task<string> GenerateJWTokenFromUserAsync(string userId)
        {
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
                return null;
            return await GenerateJWTokenFromUserAsync(user);
        }


        private async Task<bool> DeleteRefreshTokenAsync(ulong id)
        {
            var tokenFound = await _context.UserSpecificTokens.SingleOrDefaultAsync(x => x.Id == id && x.Type == UserSpecificTokenType.RefreshToken);
            if (tokenFound == null)
                return false;
            _context.UserSpecificTokens.Remove(tokenFound);

            // Remove expired tokens (cleanup)
            var expiredTokens = await _context.UserSpecificTokens.Where(x => !x.Active && x.Type == UserSpecificTokenType.RefreshToken).Take(50).ToListAsync();
            _context.UserSpecificTokens.RemoveRange(expiredTokens);

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<UserSpecificToken> GenerateRefreshTokenAsync(User user)
        {
            if (user == null)
                return null;

            // Generate token
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config.GetSection("Jwt")["RefreshExpireMinutes"]));
            var outputToken = UserSpecificToken.Generate(UserSpecificTokenType.RefreshToken, expires);

            // Save
            await _context.UserSpecificTokens.AddAsync(outputToken);
            await _context.SaveChangesAsync();
            return outputToken;
        }

        public async Task<string> GenerateJWTokenFromUserAsync(User user, string oldToken = null)
        {
            if (oldToken != null)
            {
                // Validate
                SecurityToken secTok;
                var readToken = new JwtSecurityTokenHandler().ValidateToken(oldToken, new TokenValidationParameters
                {
                    ValidIssuer = _config.GetSection("Jwt")["Issuer"],
                    ValidAudience = _config.GetSection("Jwt")["Issuer"],
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt")["Key"])),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                }, out secTok);

                if (secTok == null)
                    return null;

                var refreshToken = readToken.FindFirst("RefreshToken");
                // Check if valide
                var foundRefreshToken = await _context.UserSpecificTokens.SingleOrDefaultAsync(x => x.User.Id.Equals(user.Id) && x.Id.Equals(refreshToken.Value) && x.Type == UserSpecificTokenType.RefreshToken);
                if (foundRefreshToken == null)
                    return null;

                // Delete old one
                if (!await DeleteRefreshTokenAsync(foundRefreshToken.Id))
                    return null;
            }

            // Generate refresh token
            var newRefreshToken = await GenerateRefreshTokenAsync(user);
            if (newRefreshToken == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim("Name", user.UserName),
                new Claim("Guid", Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id),
                new Claim("RefreshToken", newRefreshToken.Token)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt")["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config.GetSection("Jwt")["ExpireMinutes"]));

            var token = new JwtSecurityToken(
                _config.GetSection("Jwt")["Issuer"],
                _config.GetSection("Jwt")["Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
