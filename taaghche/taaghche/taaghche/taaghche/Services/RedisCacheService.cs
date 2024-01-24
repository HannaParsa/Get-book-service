using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace taaghche.Services
{
    public class RedisCacheService : IDistributedCacheService
    {
        private readonly IDatabase _database;
        private readonly Settings _settings;
        public RedisCacheService(IMemoryCache memoryCache, IOptions<Settings> options)
        {
            _settings = options.Value;
            RedisConnection redisConnection = new RedisConnection();
            _database = redisConnection.Connection.GetDatabase();
        }

        public async Task<object> GetValueAsync(string key)
        {
            var result = await _database.StringGetAsync(key);
            return result;
        }

        public async Task SetValueAsync(string key, object value)
        {
            await _database.StringSetAsync(key, value.ToString(), TimeSpan.FromSeconds(_settings.RedisTimeOut));
        }

        public async Task RemoveAsync(string key)
        {
            await _database.StringGetDeleteAsync(key);
        }
    }
}
