using Eclipse.Core.Interfaces.IToken;
using Eclipse.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;


namespace Eclipse.Core.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly JWTOptions _options;
        public TokenService(IOptions<JWTOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(User user)
        {
            Claim[] claims = [new("userId", user.UserId.ToString()), new("username", user.Username)];
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(12)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
