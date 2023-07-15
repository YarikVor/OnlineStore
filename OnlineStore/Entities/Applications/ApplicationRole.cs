using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetRoles")]
public class ApplicationRole : IdentityRole<int>
{
}