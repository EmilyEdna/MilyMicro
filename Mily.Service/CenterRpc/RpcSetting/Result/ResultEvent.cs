using System;
using System.Collections.Generic;
using System.Text;
using XExten.CacheFactory;

namespace Mily.Service.CenterRpc.RpcSetting.Result
{
    public class ResultEvent
    {
        public static ResultEvent Event => new ResultEvent();
        /// <summary>
        /// 不带缓存结果
        /// </summary>
        public static Dictionary<String, Object> StaticResult { get; set; }

        public virtual void CacheResult(Dictionary<String, Object> StringCache, bool UseRedis = false)
        {
            if(UseRedis)
                Caches.RedisCacheSet

        }
        public virtual void CacheResult(Dictionary<Object, Object> ObjectCache, bool UseRedis = false)
        {

        }
        public virtual void CacheResult(dynamic DynamicCache, bool UseRedis = false)
        {

        }
    }
}
