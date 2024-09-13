using Eclipse.Core.Models;
namespace Eclipse.Core.Interfaces.IToken
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
