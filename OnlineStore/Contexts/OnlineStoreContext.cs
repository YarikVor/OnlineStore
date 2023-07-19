using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Entities;

namespace OnlineStore.Contexts;

public class OnlineStoreContext : DbContext
{
    public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options) : base(options)
    {

    }

    public DbSet<Blog> Blogs { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; } = null!;
    public DbSet<Good> Goods { get; set; } = null!;
    public DbSet<Requisition> Requisitions { get; set; } = null!;
    public DbSet<RequisitionGood> RequisitionGoods { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<SubGood> SubGoods { get; set; } = null!;
    public DbSet<AddressDelivery> AddressDeliveries { get; set; } = null!;
    public DbSet<Favourite> Favourites { get; set; } = null!;
    public DbSet<CollectionEntity> CollectionEntities { get; set; } = null!;
    public DbSet<CollectionEntityGood> CollectionEntityGoods { get; set; } = null!;
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Entity<ApplicationUserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        ExcludeFromMigrations<ApplicationRole>();
        ExcludeFromMigrations<ApplicationRoleClaim>();
        ExcludeFromMigrations<ApplicationUser>();
        ExcludeFromMigrations<ApplicationUserClaim>();
        ExcludeFromMigrations<ApplicationUserLogin>();
        ExcludeFromMigrations<ApplicationUserRole>();
        ExcludeFromMigrations<ApplicationUserToken>();

        EntityTypeBuilder<TEntity> Entity<TEntity>() where TEntity : class
        {
            return modelBuilder.Entity<TEntity>();
        }

        void ExcludeFromMigrations<TEntity>() where TEntity : class
        {
            Entity<TEntity>().ToTable(tb => tb.ExcludeFromMigrations());
        }
    }
}