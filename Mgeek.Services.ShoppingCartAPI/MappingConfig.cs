using AutoMapper;


namespace Mgeek.Services.ShoppingCartAPI;

public class MappingConfig
{
    public static MapperConfiguration Registermaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            });
        return mappingConfig;
    }
}