using System;
using System.Collections.Generic;
using System.Text;
using XExten.CacheFactory;
using XExten.XCore;

namespace Mily.Service.CenterRpc.RpcSetting.Result
{
    public class ResultEvent
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        public static ResultEvent Event => new ResultEvent();
        /// <summary>
        /// 不带缓存结果
        /// </summary>
        public static Dictionary<String, Object> StaticResult { get; set; }
        /// <summary>
        /// 结果缓存
        /// </summary>
        /// <param name="KeyPress"></param>
        /// <param name="StringCache"></param>
        /// <param name="UseRedis"></param>
        public virtual void CacheResult(String KeyPress, Dictionary<String, Object> StringCache, bool UseRedis = false)
        {
            StringCache["ResultData"] = StringCache["ResultData"]?.ToString().ToModel<Object>();
            if (UseRedis)
                Caches.RedisCacheSet(KeyPress, StringCache, 10, true);
            else
                Caches.RunTimeCacheSet(KeyPress, StringCache, 10, true);

        }
        /// <summary>
        /// 结果缓存
        /// </summary>
        /// <param name="KeyPress"></param>
        /// <param name="ObjectCache"></param>
        /// <param name="UseRedis"></param>
        public virtual void CacheResult(String KeyPress, Dictionary<Object, Object> ObjectCache, bool UseRedis = false)
        {
            ObjectCache["ResultData"] = ObjectCache["ResultData"].ToString().ToModel<Object>();
            if (UseRedis)
                Caches.RedisCacheSet(KeyPress, ObjectCache, 10, true);
            else
                Caches.RunTimeCacheSet(KeyPress, ObjectCache, 10, true);
        }
        /// <summary>
        /// 结果缓存
        /// </summary>
        /// <param name="KeyPress"></param>
        /// <param name="DynamicCache"></param>
        /// <param name="UseRedis"></param>
        public virtual void CacheResult(String KeyPress, dynamic DynamicCache, bool UseRedis = false)
        {
            DynamicCache.ResultData = DynamicCache.ResultData.ToString().ToModel<Object>();
            if (UseRedis)
                Caches.RedisCacheSet(KeyPress, DynamicCache, 10, true);
            else
                Caches.RunTimeCacheSet(KeyPress, DynamicCache, 10, true);
        }
        /// <summary>
        /// 获取结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="KeyPress"></param>
        /// <param name="UseRedis"></param>
        /// <returns></returns>
        public virtual T GetCacheResult<T>(String KeyPress, bool UseRedis = false)
        {
            if (UseRedis)
                return Caches.RedisCacheGet<T>(KeyPress);
            else
                return Caches.RunTimeCacheGet<T>(KeyPress);
        }
    }
}
