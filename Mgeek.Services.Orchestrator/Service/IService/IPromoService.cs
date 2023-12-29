using Mgeek.Services.Orchestrator.Models;

namespace Mgeek.Services.Orchestrator.Service.IService;

public interface IPromoService
{
    Task<ResponseDto?> DecrementPromo(string name, string token);
    Task<ResponseDto?> IncrementPromo(string name, string token);
}