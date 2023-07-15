using System.Security.Cryptography.X509Certificates;
using AutoMapper;

namespace OnlineStore.Mappers;

public class OnlineShopMapperConfigurationExpression : MapperConfigurationExpression
{
    public OnlineShopMapperConfigurationExpression()
    {
        var profiles = new Profile[]
        {
            new CategoryProfile()
        };
        
        AddProfiles(profiles);
    }
}