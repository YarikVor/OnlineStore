using System.Security.Claims;

namespace OnlineStore;

public interface ITokenGenerator
{
    string GenerateToken(IEnumerable<Claim> claims, TokenType tokenType);
}