namespace OnlineStore.Controllers;

public class UserEditPersonalDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool SubscribeNews { get; set; }
}