using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Controllers;

public class RegistrationDto
{
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = null!;
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    [StringLength(maximumLength: 50, MinimumLength = 8)]
    public string Password { get; set; } = null!;
}