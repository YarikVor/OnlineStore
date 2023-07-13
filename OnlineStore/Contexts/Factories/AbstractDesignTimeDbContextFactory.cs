using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnlineStore.Contexts.Factories;

public abstract class AbstractDesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
    protected readonly IConfiguration Configuration;
    protected readonly Type DbContextType = typeof(TContext);

    protected AbstractDesignTimeDbContextFactory()
    {
        var configurationManager = new ConfigurationManager();
        configurationManager.AddJsonFile("appsettings.json");
        Configuration = configurationManager;
    }

    public virtual TContext CreateDbContext(string[] args)
    {
        var connectionString = Configuration.GetRequiredDefaultConnectionString();

        var options = new DbContextOptionsBuilder<TContext>()
            .UseNpgsql(connectionString)
            .Options;

        return (TContext)Activator.CreateInstance(DbContextType, options)!;
    }
}