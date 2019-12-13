using SqlSugar;
using System;
using System.Collections.Generic;
using XCache = XExten.CacheFactory;

namespace Mily.DbCore.Caches
{
    public class DbCache : XCache.Caches,ICacheService
    {
        public void Add<T>(string key, T value)
        {
            RedisCacheSet(key, value);
        }

        public void Add<T>(string key, T value, int cacheDurationInSeconds)
        {
            RedisCacheSet(key, value, cacheDurationInSeconds, true);
        }

        public bool ContainsKey<T>(string key)
        {
            return RedisCacheGet<T>(key) != null ? true : false;
        }

        public T Get<T>(string key)
        {
            return RedisCacheGet<T>(key);
        }

        public IEnumerable<string> GetAllKey<T>()
        {
            return null;
        }

        public T GetOrCreate<T>(string cacheKey, Func<T> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (this.ContainsKey<T>(cacheKey))
            {
                return this.Get<T>(cacheKey);
            }
            else
            {
                var result = create();
                Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<T>(string key)
        {
            RedisCacheRemove(key);
        }
    }
}