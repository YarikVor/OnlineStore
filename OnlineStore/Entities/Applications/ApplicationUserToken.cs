using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetUserTokens")]
public class ApplicationUserToken : IdentityUserToken<int>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public override int UserId { get; set; }
}