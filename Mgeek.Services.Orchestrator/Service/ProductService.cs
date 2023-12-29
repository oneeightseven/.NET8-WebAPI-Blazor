using System.Text;
using Mgeek.Services.Orchestrator.Models;
using Mgeek.Services.Orchestrator.Models.ProductAPI;
using Mgeek.Services.Orchestrator.Service.IService;
using Newtonsoft.Json;

namespace Mgeek.Services.Orchestrator.Service;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClient;

    public ProductService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ResponseDto?> DecreaseProducts(List<StockDto> stocksDto, string token)
    {
        var client = _httpClient.CreateClient("Orkestrator");
        HttpRequestMessage message = new();
        message.Headers.Add("Accept", "application/json");
        message.Headers.Add("Authorization",$"Bearer {token}");
        message.Method = HttpMethod.Post;
        message.Content =new StringContent(JsonConvert.SerializeObject(stocksDto), Encoding.UTF8, "application/json");
        message.RequestUri = new Uri($"http://localhost:5026/api/ProductAPI/DecreaseProducts/");
        var response = await client.SendAsync(message);
        var apiContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
    }
}