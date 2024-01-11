using Newtonsoft.Json;

namespace Mgeek.Services.ShoppingCartAPI.Service;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClient;

    public ProductService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        var client = _httpClient.CreateClient("Product");
        var response = await client.GetAsync($"/api/ProductApi/GetAll");
        var apiContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        
        if (result.IsSuccess)
            return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(result.Result)!)!;

        return null;
    }
}