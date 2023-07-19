using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Entities;

namespace OnlineStore.Contexts;

public class ApplicationContext
    : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        int,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken
    >
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Entity<ApplicationUserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

        var roles = new ApplicationRole[]
        {
            new ApplicationRole(ApplicationRole.User, 1),
            new ApplicationRole(ApplicationRole.Admin, 2),
        };

        Entity<ApplicationRole>().HasData(roles);

        EntityTypeBuilder<TEntity> Entity<TEntity>() where TEntity : class
        {
            return builder.Entity<TEntity>();
        }
    }
}