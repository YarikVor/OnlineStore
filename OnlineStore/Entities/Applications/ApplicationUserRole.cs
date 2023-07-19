using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetUserRoles")]
public class ApplicationUserRole : IdentityUserRole<int>
{
    public override int UserId { get; set; }
    public override int RoleId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; } = null!;
    
    [ForeignKey(nameof(RoleId))]
    public virtual ApplicationRole Role { get; set; } = null!;
}