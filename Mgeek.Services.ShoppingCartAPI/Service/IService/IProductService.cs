using Mgeek.Services.ShoppingCartAPI.Models.Dto;

namespace Mgeek.Services.ShoppingCartAPI.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProducts();
}