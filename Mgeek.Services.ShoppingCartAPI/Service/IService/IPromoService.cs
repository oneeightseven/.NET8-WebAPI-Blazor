namespace Mgeek.Services.ShoppingCartAPI.Service.IService;

public interface IPromoService
{
    Task<PromocodeDto> GetPromo(string name);
}