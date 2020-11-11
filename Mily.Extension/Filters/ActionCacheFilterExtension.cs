using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mily.Extension.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.CacheFactory;
using XExten.CacheFactory.RedisCache;
using XExten.XCore;

namespace Mily.Extension.Filters
{
    /// <summary>
    /// Api缓存
    /// </summary>
    public static class ActionCacheFilterExtension
    {

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="Context"></param>
        internal static void OnExcuting(this ActionExecutingContext Context)
        {
            var Req = Context.HttpContext.Request;
            if (Req.Headers.ContainsKey("Cache"))
            {
                var CacheValue = Req.Headers["Cache"];
                string Query = Req.Query.Count() != 0 ?
                    Req.Query.ToDictionary(t => t.Key, t => t.Value.ToString()).ToJson() :
                    Req.Form.ToDictionary(t => t.Key, t => t.Value.ToString()).ToJson();
                var SignKey = $@"{Req.Host.Value}|{Req.Path.Value}|{Req.Method}|{Query}|{CacheValue}".ToMD5();
                var Result = Caches.RedisCacheGet<ResultCondition>(SignKey);
                if (Result != null)
                {
                    Context.HttpContext.Response.Headers.Add("CacheType", "Redis");
                    Context.Result = new ObjectResult(Result);
                }
            }
        }

        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="Context"></param>
        internal static void OnExcuted(this ActionExecutedContext Context)
        {
            var Res = Context.HttpContext.Response;
            var Req = Context.HttpContext.Request;
            if (Res.Headers.ContainsKey("CacheType")) return;
            ResultCondition Condition = ResultCondition.Instance(Item =>
            {
                Item.IsSuccess = true;
                Item.StatusCode = Context.HttpContext.Response.StatusCode;
                Item.ResultData = (Context.Result as ObjectResult).Value;
                Item.Info = "执行成功!";
                Item.ServerDate = DateTime.Now.ToFmtDate(1);
            });
            if (Req.Headers.ContainsKey("Cache"))
            {
                var CacheValue = Req.Headers["Cache"];
                string Query = Req.Query.Count() != 0 ? 
                    Req.Query.ToDictionary(t => t.Key, t => t.Value.ToString()).ToJson() : 
                    Req.Form.ToDictionary(t => t.Key, t => t.Value.ToString()).ToJson();
                var SignKey = $@"{Req.Host.Value}|{Req.Path.Value}|{Req.Method}|{Query}|{CacheValue}".ToMD5();
                Caches.RedisCacheSet(SignKey, Condition, 1);
            }
            Context.Result = new ObjectResult(Condition);

        }
    }
}
