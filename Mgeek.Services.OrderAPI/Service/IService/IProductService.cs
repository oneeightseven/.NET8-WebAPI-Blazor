using Mgeek.Services.OrderAPI.Models.Dto;

namespace Mgeek.Services.OrderAPI.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProducts();
}