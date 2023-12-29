using AutoMapper;
using Mgeek.Services.OrderAPI.Models;
using Mgeek.Services.OrderAPI.Models.Dto;

namespace Mgeek.Services.OrderAPI;

public class MappingConfig
{
    public static MapperConfiguration Registermaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            config.CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
            config.CreateMap<Order, OrderDto>().IncludeAllDerived().ReverseMap();
        });
        return mappingConfig;
    }
}