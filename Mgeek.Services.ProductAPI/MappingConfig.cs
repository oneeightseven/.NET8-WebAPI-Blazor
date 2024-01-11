namespace Mgeek.Services.ProductAPI;
public class MappingConfig
{
    public static MapperConfiguration Registermaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().IncludeAllDerived().ReverseMap();
                config.CreateMap<Smartphone, SmartphoneDto>().IncludeAllDerived().ReverseMap();
                config.CreateMap<Laptop, LaptopDto>().IncludeAllDerived().ReverseMap();
                config.CreateMap<Stock, StockDto>().ReverseMap();
            });
        return mappingConfig;
    }
}