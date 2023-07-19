namespace OnlineStore.Controllers;

public class UserFullInfoDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool SubscribedToNews { get; set; }
    public string Role { get; set; }
}