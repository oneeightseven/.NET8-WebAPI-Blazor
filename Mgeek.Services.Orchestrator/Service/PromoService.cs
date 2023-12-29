using System.Text;
using Mgeek.Services.Orchestrator.Models;
using Mgeek.Services.Orchestrator.Service.IService;
using Newtonsoft.Json;

namespace Mgeek.Services.Orchestrator.Service;

public class PromoService : IPromoService
{
    private readonly IHttpClientFactory _httpClient;

    public PromoService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<ResponseDto?> DecrementPromo(string name, string token)
    {
        var client = _httpClient.CreateClient("Orkestrator");
        HttpRequestMessage message = new();
        message.Headers.Add("Accept", "application/json");
        message.Headers.Add("Authorization",$"Bearer {token}");
        message.Method = HttpMethod.Get;
        message.RequestUri = new Uri($"http://localhost:5033/api/PromocodeApi/DecrementPromo/" + name);
        var response = await client.SendAsync(message);
        var apiContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
    }

    public async Task<ResponseDto?> IncrementPromo(string name, string token)
    {
        var client = _httpClient.CreateClient("Orkestrator");
        HttpRequestMessage message = new();
        message.Headers.Add("Accept", "application/json");
        message.Headers.Add("Authorization",$"Bearer {token}");
        message.Method = HttpMethod.Get;
        message.RequestUri = new Uri($"http://localhost:5033/api/PromocodeApi/IncrementPromo/" + name);
        var response = await client.SendAsync(message);
        var apiContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
    }
}