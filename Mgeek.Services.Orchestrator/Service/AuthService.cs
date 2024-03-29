using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mgeek.Services.Orchestrator.Service;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClient;

    public AuthService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<string> Login()
    {
        LoginRequestDto loginRequest = new()
        {
            UserName = "d1s8h7h@vk.com",
            Password = "123Mudak228!"
        };
        var client = _httpClient.CreateClient("Orkestrator");
        HttpRequestMessage message = new();
        message.Headers.Add("Accept", "application/json");
        message.Method = HttpMethod.Post;
        message.Content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
        message.RequestUri = new Uri($"http://localhost:5216/api/AuthAPI/login/");
        var response = await client.SendAsync(message);
        var apiContent = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(apiContent);
        return json["result"]["token"].ToString();
    }
}