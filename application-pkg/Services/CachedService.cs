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
        ValidateKey(key);
        var values = await _distributedCache.GetStringAsync(key);
        return values is null ? null : JsonSerializer.Deserialize<List<T>>(values);
    }

    public async Task<T?> GetItemAsync(string key)
    {
        ValidateKey(key);
        var value = await _distributedCache.GetStringAsync(key);
        return value is null ? null : JsonSerializer.Deserialize<T>(value);
    }

    public async Task RemoveCached(string key)
    {
        ValidateKey(key);
        await _distributedCache.RemoveAsync(key);
    }

    public async Task<bool> SetCollectionAsync(string key, List<T> values)
    {
        ValidateKey(key);
        ValidateListValueNull(values);
        var valuesJson = JsonSerializer.Serialize<List<T>>(values);
        await _distributedCache.SetStringAsync(key, valuesJson, _options);
        return true;
    }

    public async Task<bool> SetItemAsync(string key, T value)
    {
        ValidateKey(key);
        ValidateValueNull(value);
        var valueJson = JsonSerializer.Serialize<T>(value);
        await _distributedCache.SetStringAsync(key, valueJson, _options);
        return true;
    }

    private static void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key inválida!");
    }

    private static void ValidateValueNull(T? value)
    {
        if (value == null) throw new ArgumentException("key inválida!");
    }

    private static void ValidateListValueNull(List<T>? value)
    {
        if (value == null) throw new ArgumentException("key inválida!");
    }
}
