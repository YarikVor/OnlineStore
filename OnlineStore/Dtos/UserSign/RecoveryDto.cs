namespace OnlineStore.Controllers;

public class RecoveryDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}