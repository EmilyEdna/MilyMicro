using SqlSugar;
using System;
using System.Collections.Generic;
using XExten.CacheFactory.RedisCache;

namespace Mily.DbCore.Caches
{
    public class DbCache : RedisCaches, ICacheService
    {
        public void Add<T>(string key, T value)
        {
            StringSet(key, value);
        }

        public void Add<T>(string key, T value, int cacheDurationInSeconds)
        {
            StringSet(key, value, (DateTime.Now.AddSeconds(cacheDurationInSeconds) - DateTime.Now));
        }

        public bool ContainsKey<T>(string key)
        {
            return StringGet<T>(key) != null ? true : false;
        }

        public T Get<T>(string key)
        {
            return StringGet<T>(key);
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
            KeyDelete(key);
        }
    }
}