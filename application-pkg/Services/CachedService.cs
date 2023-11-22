namespace application_pkg.Services;

public class CachedService<T> : ICachedService<T> where T : class
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _options;
    public CachedService(IDistributedCache distributedCache)
    {
        _options = new DistributedCacheEntryOptions()
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                      .SetSlidingExpiration(TimeSpan.FromMinutes(30));

        _distributedCache = distributedCache;
    }

    public async Task<List<T>?> GetCollectionAsync(string key)
    {
        var values = await _distributedCache.GetStringAsync(key);
        return values is null ? null : JsonSerializer.Deserialize<List<T>>(values);
    }

    public async Task<T?> GetItemAsync(string key)
    {
        var value = await _distributedCache.GetStringAsync(key);
        return value is null ? null : JsonSerializer.Deserialize<T>(value);
    }

    public async Task RemoveCached(string key) => await _distributedCache.RemoveAsync(key);

    public async Task<bool> SetCollectionAsync(string key, List<T> values)
    {
        try
        {
            var valuesJson = JsonSerializer.Serialize<List<T>>(values);
            await _distributedCache.SetStringAsync(key, valuesJson, _options);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SetItemAsync(string key, T value)
    {
        try
        {
            var valueJson = JsonSerializer.Serialize<T>(value);
            await _distributedCache.SetStringAsync(key, valueJson, _options);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
