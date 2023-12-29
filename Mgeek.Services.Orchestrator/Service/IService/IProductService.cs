using Mgeek.Services.Orchestrator.Models;
using Mgeek.Services.Orchestrator.Models.ProductAPI;

namespace Mgeek.Services.Orchestrator.Service.IService;

public interface IProductService
{
    Task<ResponseDto?> DecreaseProducts(List<StockDto> stocksDto, string token);
}