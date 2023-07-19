using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;

namespace OnlineStore.Mappers;

public class OnlineShopMapperConfigurationExpression : MapperConfigurationExpression
{
    public OnlineShopMapperConfigurationExpression()
    {
        var profiles = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                t.IsSubclassOf(typeof(Profile))
                && !t.IsSubclassOf(typeof(MapperConfigurationExpression)))
            .Select(t => (Profile)Activator.CreateInstance(t)!);

        AddProfiles(profiles);
    }
}