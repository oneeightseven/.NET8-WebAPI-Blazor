using Mgeek.Services.ShoppingCartAPI.Models.Dto;
using Mgeek.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mgeek.Services.ShoppingCartAPI.Service;

public class PromoService : IPromoService
{
    private readonly IHttpClientFactory _httpClient;

    public PromoService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<PromocodeDto> GetPromo(string name)
    {
        var client = _httpClient.CreateClient("Promo");
        var response = await client.GetAsync($"/api/PromocodeApi/GetByName/{name}");
        var apiContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        if (result!.IsSuccess)
            return JsonConvert.DeserializeObject<PromocodeDto>(Convert.ToString(result.Result)!)!;
        return null!;
    }
}