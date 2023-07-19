using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetUsers")]

public class ApplicationUser : IdentityUser<int>
{

    [Key]
    [PersonalData]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int Id { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
    
    public bool SubscribedToNews { get; set; }

    [NotMapped]
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
    [NotMapped]
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
    [NotMapped]
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
    [NotMapped]
    [InverseProperty("User")]
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}