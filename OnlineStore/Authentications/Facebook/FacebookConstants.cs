using Microsoft.AspNetCore.Authentication.Facebook;

namespace OnlineStore.Authentications.Facebook;

public static class FacebookConstants
{
    public const string AuthenticationScheme = "Facebook";
    public const string DisplayName = "Facebook";
    public const string AuthorizationEndpoint = "https://www.facebook.com/dialog/oauth";
    public const string TokenEndpoint = "https://graph.facebook.com/oauth/access_token";
    public const string UserInformationEndpoint = "https://graph.facebook.com/me";
}