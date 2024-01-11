using StackExchange.Redis;

namespace Mgeek.Services.OrderAPI.Service;

public class CacheService : ICacheService
{
    private IDatabase _cacheDb;
    
    public CacheService()
    {
        var redis = ConnectionMultiplexer.Connect("localhost:6380");
        _cacheDb = redis.GetDatabase();
    }
    
    public T Get<T>(string key)
    {
        var value = _cacheDb.StringGet(key);
        if (!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<T>(value);

        return default;
    }

    public bool Set<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);

        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expirtyTime);
    }

    public object Remove(string key)
    {
        var exist = _cacheDb.KeyExists(key);
        if (exist)
            return _cacheDb.KeyDelete(key);

        return false;
    }
}