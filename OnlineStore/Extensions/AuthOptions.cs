using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OnlineStore;

public static class AuthOptions
{
    public const string ISSUER = "MyAuthServer";
    public const string AccessClient = "access";
    public const string Client = "access";
    public const string RefreshClient = "refresh";
    const string KEY = "secretkeyis123412341234";
    public const int LIFETIME = 1;
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}