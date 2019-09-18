using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XExten.CacheFactory.RedisCache;

namespace Mily.Extension.Caches
{
    public class CacheFacoty
    {
        /// <summary>
        /// 缓存中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static async Task<T> GetCache<T>(string CacheKey)
        {
            return await RedisCaches.StringGetAsync<T>(CacheKey);
        }
        /// <summary>
        /// 删除某个缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        public static async Task RemoveCache(string CacheKey)
        {
            await RedisCaches.KeyDeleteAsync(CacheKey);
        }
        /// <summary>
        /// 删除所有数据
        /// </summary>
        public static async Task RemoveCache()
        {
            await RedisCaches.DeleteRedisDataBaseAsync();
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="CacheKey"></param>
        /// <param name="hours"></param>
        public static async Task WriteCache<T>(T obj, string CacheKey, int hours)
        {
            await RedisCaches.StringSetAsync<T>(CacheKey, obj, (DateTime.Now.AddHours(hours) - DateTime.Now));
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="CacheKey"></param>
        public static async Task WriteCache<T>(T obj, string CacheKey)
        {
            await RedisCaches.StringSetAsync<T>(CacheKey, obj);
        }
    }
}
