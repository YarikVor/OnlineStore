using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Contexts;

[Table("AspNetRoleClaims")]
public class ApplicationRoleClaim : IdentityRoleClaim<int>
{
}