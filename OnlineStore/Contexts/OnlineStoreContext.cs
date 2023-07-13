using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Entities;

namespace OnlineStore.Contexts;

public class OnlineStoreContext: DbContext
{
    public DbSet<Blog> Blog { get; set; } = null!;
    public DbSet<Category> Category { get; set; } = null!;
    public DbSet<DeliveryMethod> DeliveryMethod { get; set; } = null!;
    public DbSet<Good> Good { get; set; } = null!;
    public DbSet<Requisition> Requisitions { get; set; } = null!;
    public DbSet<RequisitionGood> RequisitionGood { get; set; } = null!;
    public DbSet<Response> Response { get; set; } = null!;
    public DbSet<SubGood> SubGood { get; set; } = null!;

    public OnlineStoreContext(DbContextOptions options): base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Entity<ApplicationUserRole>().HasKey(ur => new {ur.UserId, ur.RoleId});
        Entity<ApplicationUserClaim>().HasKey(ur => new {ur.UserId});
        Entity<ApplicationUserToken>().HasKey(ur => new {ur.UserId});

        ExcludeFromMigrations<ApplicationRole>();
        ExcludeFromMigrations<ApplicationRoleClaim>();
        ExcludeFromMigrations<ApplicationUser>();
        ExcludeFromMigrations<ApplicationUserClaim>();
        ExcludeFromMigrations<ApplicationUserLogin>();
        ExcludeFromMigrations<ApplicationUserRole>();
        ExcludeFromMigrations<ApplicationUserToken>();
        
        base.OnModelCreating(modelBuilder);

        EntityTypeBuilder<TEntity> Entity<TEntity>() where TEntity : class 
            => modelBuilder.Entity<TEntity>();
        void ExcludeFromMigrations<TEntity>() where TEntity : class 
            => Entity<TEntity>().ToTable(tb => tb.ExcludeFromMigrations()); 
    }
}