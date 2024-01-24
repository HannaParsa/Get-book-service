namespace taaghche.Services
{
    public interface IDistributedCacheService
    {
        Task<object> GetValueAsync(string key);
        Task SetValueAsync(string key, object value);
        Task RemoveAsync(string key);
    }
}
