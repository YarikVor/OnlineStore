using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetUserTokens")]
public class ApplicationUserToken : IdentityUserToken<int>
{
}