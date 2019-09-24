using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Jmwoo.Common.Caching
{
    public interface IBaseDistributedCacheSingle<T>
    {
        Task<T> Set(T item);
        Task<T> Get();
    }

    public interface IBaseDistributedCache<T>
    {
        Task<T> Set(T item, string key);
        Task<T> Get(string key);
    }

    public abstract class BaseDistributedCache<T> : IBaseDistributedCache<T>, IBaseDistributedCacheSingle<T>
    {
        private readonly IDistributedCache _cache;

        protected BaseDistributedCache(
            IDistributedCache cache
        )
        {
            _cache = cache;
        }

        public virtual DistributedCacheEntryOptions Options => new DistributedCacheEntryOptions();

        public virtual string CachePrefix
        {
            get
            {
                var type = typeof(T);
                return $"{type.Namespace}.{type.Name}";
            }
        }

        public async Task<T> Get(string key)
        {
            var cacheKey = MakeCacheKey(key);

            var blob = await _cache.GetStringAsync(cacheKey);

            if (blob != null)
            {
                return JsonConvert.DeserializeObject<T>(blob);
            }

            return default;
        }

        public async Task<T> Set(T item, string key)
        {
            var cacheKey = MakeCacheKey(key);

            var blob = JsonConvert.SerializeObject(item);

            await _cache.SetStringAsync(cacheKey, blob, Options);
            return item;
        }

        public async Task<T> Get()
        {
            return await this.Get(null);
        }

        public async Task<T> Set(T item)
        {
            return await this.Set(item, null);
        }

        public string MakeCacheKey(string key) => string.IsNullOrEmpty(key) ? CachePrefix : $"{CachePrefix}.{key}";
    }
}
