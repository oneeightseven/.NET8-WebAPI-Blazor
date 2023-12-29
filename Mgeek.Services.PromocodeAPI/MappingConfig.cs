using AutoMapper;
using Mgeek.Services.PromocodeAPI.Models;
using Mgeek.Services.PromocodeAPI.Models.Dto;

namespace Mgeek.Services.PromocodeAPI;

public class MappingConfig
{
    public static MapperConfiguration Registermaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Promocode, PromocodeDto>().ReverseMap();
        });
        return mappingConfig;
    }
}