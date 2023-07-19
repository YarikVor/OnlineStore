namespace OnlineStore.Controllers;

public class UserTokenDto
{
    public UserFullInfoDto User { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class UserAccessTokenDto
{
    public UserFullInfoDto User { get; set; }
    public string AccessToken { get; set; }
}