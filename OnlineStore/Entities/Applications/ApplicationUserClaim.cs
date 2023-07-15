using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetUserClaims")]
public class ApplicationUserClaim : IdentityUserClaim<int>
{
}