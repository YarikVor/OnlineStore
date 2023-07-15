namespace OnlineStore;

public static class ConfigurationExtensions
{
    public const string DefaultName = "Default";

    public static string GetRequiredDefaultConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString(DefaultName)
               ?? throw new KeyNotFoundException($"Key [ConnectionStrings:{DefaultName}] isn't available!");
    }
}