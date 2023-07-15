using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetUserLogins")]
public class ApplicationUserLogin : IdentityUserLogin<int>
{
    [Key]
    [Column(Order = 0)]
    [ForeignKey(nameof(ApplicationUser))]
    public override int UserId { get; set; }


    public virtual ApplicationUser ApplicationUser { get; set; }
}