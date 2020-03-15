using System;
using System.Collections.Generic;
using System.Text;
using XExten.CacheFactory;
using XExten.XCore;

namespace Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.Result
{
    public class ResultProcess
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        public static ResultProcess Event => new ResultProcess();
        /// <summary>
        /// 结果缓存
        /// </summary>
        /// <param name="KeyPress"></param>
        /// <param name="StringCache"></param>
        /// <param name="CacheTime"></param>
        /// <param name="UseRedis"></param>
        public virtual void CacheProcess(String KeyPress, Dictionary<String, Object> StringCache, int CacheTime = 5, bool UseRedis = false)
        {
            StringCache["ResultData"] = StringCache["ResultData"]?.ToString().ToModel<Object>();
            if (UseRedis)
                Caches.RedisCacheSet(KeyPress, StringCache, CacheTime, true);
            else
                Caches.RunTimeCacheSet(KeyPress, StringCache, CacheTime, true);
        }
        /// <summary>
        /// 结果缓存
        /// </summary>
        /// <param name="KeyPress"></param>
        /// <param name="ObjectCache"></param>
        /// <param name="CacheTime"></param>
        /// <param name="UseRedis"></param>
        public virtual void CacheProcess(String KeyPress, Dictionary<Object, Object> ObjectCache, int CacheTime = 5, bool UseRedis = false)
        {
            ObjectCache["ResultData"] = ObjectCache["ResultData"].ToString().ToModel<Object>();
            if (UseRedis)
                Caches.RedisCacheSet(KeyPress, ObjectCache, CacheTime, true);
            else
                Caches.RunTimeCacheSet(KeyPress, ObjectCache, CacheTime, true);
        }
        /// <summary>
        /// 结果缓存
        /// </summary>
        /// <param name="KeyPress"></param>
        /// <param name="DynamicCache"></param>
        /// <param name="CacheTime"></param>
        /// <param name="UseRedis"></param>
        public virtual void CacheProcess(String KeyPress, dynamic DynamicCache, int CacheTime = 5, bool UseRedis = false)
        {
            DynamicCache.ResultData = DynamicCache.ResultData.ToString().ToModel<Object>();
            if (UseRedis)
                Caches.RedisCacheSet(KeyPress, DynamicCache, CacheTime, true);
            else
                Caches.RunTimeCacheSet(KeyPress, DynamicCache, CacheTime, true);
        }
        /// <summary>
        /// 获取结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="KeyPress"></param>
        /// <param name="UseRedis"></param>
        /// <returns></returns>
        public virtual T GetCacheProcess<T>(String KeyPress, bool UseRedis = false)
        {
            if (UseRedis)
                return Caches.RedisCacheGet<T>(KeyPress);
            else
                return Caches.RunTimeCacheGet<T>(KeyPress);
        }
    }
}
