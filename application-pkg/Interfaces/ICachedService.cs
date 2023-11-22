namespace application_pkg.Interfaces;

public interface ICachedService<T> where T : class
{
    Task<T?> GetItemAsync(string key);
    Task<List<T>?> GetCollectionAsync(string key);
    Task<bool> SetCollectionAsync(string key, List<T> values);
    Task<bool> SetItemAsync(string key, T value);
    Task RemoveCached(string key);
}
