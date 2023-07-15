using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
}