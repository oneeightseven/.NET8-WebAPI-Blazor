namespace Mgeek.Services.ShoppingCartAPI.Service.IService;

public interface ICacheService
{
    T Get<T>(string key);
    bool Set<T>(string key, T value, DateTimeOffset expirationTime);
    object Remove(string key);
}