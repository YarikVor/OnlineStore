using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Entities;

[Table("AspNetRoles")]
public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string name): base(name)
    {
        NormalizedName = name.ToUpper();
    }

    public ApplicationRole(string name, int id): this(name)
    {
        Id = id;
    }

    public const string User = "user";
    public const string Admin = "admin";
    public const string UserAndAdmin = $"{User},{Admin}";

    
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}