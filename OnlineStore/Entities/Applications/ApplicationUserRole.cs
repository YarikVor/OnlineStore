using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetUserRoles")]
public class ApplicationUserRole : IdentityUserRole<int>
{
    [Key]
    [Column(Order = 0)]
    [ForeignKey(nameof(ApplicationUser))]
    public override int UserId { get; set; }
    
    public virtual ApplicationUser ApplicationUser { get; set; }
    
    [Key]
    [Column(Order = 1)]
    [ForeignKey(nameof(ApplicationRole))]
    public override int RoleId { get; set; }
    
    public virtual ApplicationRole ApplicationRole { get; set; }
}