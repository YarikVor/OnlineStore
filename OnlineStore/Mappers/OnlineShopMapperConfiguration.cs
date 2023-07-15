using AutoMapper;

namespace OnlineStore.Mappers;

public class OnlineShopMapperConfiguration : MapperConfiguration
{
    public OnlineShopMapperConfiguration() : base(new OnlineShopMapperConfigurationExpression())
    {
        
    }
}