using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace taaghche.Services
{
    public class InMemoryCacheService : IInMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Settings _settings;
        public InMemoryCacheService(IMemoryCache memoryCache, IOptions<Settings> options)
        {
            _memoryCache = memoryCache;
            _settings = options.Value;
        }

        public object GetValue(string key)
        {
            if (_memoryCache.TryGetValue(key, out object result))
                return result;
            return null;
        }

        public void SetValue(string key, object value)
        {
            _memoryCache.Set(key, value.ToString(),
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.MemoryTimeOut)));
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
