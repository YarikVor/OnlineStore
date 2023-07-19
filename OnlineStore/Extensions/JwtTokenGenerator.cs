using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace OnlineStore;

public class JwtTokenGenerator : ITokenGenerator
{
    public string GenerateToken(IEnumerable<Claim> claims, TokenType tokenType)
    {
        var audience = tokenType switch
        {
            TokenType.Access => AuthOptions.AccessClient,
            TokenType.Refresh => AuthOptions.RefreshClient,
            _ => throw new Exception()
        };

        var timeLife = tokenType switch
        {
            TokenType.Access => new TimeSpan(0, 30, 0),
            TokenType.Refresh => new TimeSpan(14, 0, 0, 0),
            _ => throw new Exception()
        };

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.Client,
            notBefore: DateTime.Now,
            claims: claims,
            expires: DateTime.Now.Add(timeLife),
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256
            )
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }
}