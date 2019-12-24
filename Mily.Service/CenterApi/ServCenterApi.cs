using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Service.CenterApi.CenterAop;
using Mily.Service.CenterApi.ViewModel;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.XCore;
using System.Collections.Generic;

namespace Mily.Service.CenterApi
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "/Center")]
    [CenterFilter]
    public class ServCenterApi
    {
        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <returns></returns>
        [Get]
        public async Task<Object> GetServer()
        {
            return await Caches.MongoDBCachesGetAsync<ServerCondition>(t => true);
        }
        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        [Post]
        public async Task<String> InsertRoute(IHttpContext Context)
        {
            List<ServerCondition> Conditions = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<List<ServerCondition>>();
            Conditions.ForEach(Item =>
            {
                Caches.MongoDbCacheUpdateAsync<ServerCondition>(t => t.ServiceName == Item.ServiceName, "Route", Item.Route);
            });
            return await Task.FromResult("添加成功!");
        }
        /// <summary>
        /// 启用HTTP负载
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        [Post]
        public async Task<String> StarUseHttp(IHttpContext Context)
        {
            ServerCondition Conditions = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<ServerCondition>();
            var Condition = Caches.MongoDBCachesGet<ServerCondition>(t => Conditions.ServiceName == t.ServiceName).Any(t => t.HttpPort.IsNullOrEmpty());
            if (Condition)
                return await Task.FromResult("请先添加HTTP端口号!");
            else
                Caches.MongoDbCacheUpdate<ServerCondition>(t => t.ServiceName == Conditions.ServiceName, "UseHttp", Conditions.UseHttp.ToString());
            return await Task.FromResult(Conditions.UseHttp ? "启用成功!" : "禁用成功!");
        }
        /// <summary>
        /// 添加HTTP服务
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        [Post]
        public async Task<String> SaveHttpServer(IHttpContext Context)
        {
            ServerCondition Condition = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<ServerCondition>();
            Caches.MongoDbCacheUpdate<ServerCondition>(t => t.Key == Condition.Key, "HttpPort", Condition.HttpPort);
            return await Task.FromResult("保存成功!");
        }
        /// <summary>
        /// 添加负载权重
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        [Post]
        public async Task<String> SaveWeightRabin(IHttpContext Context)
        {
            ServerCondition Condition = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<ServerCondition>();
            Caches.MongoDbCacheUpdate<ServerCondition>(t => t.Key == Condition.Key, "HttpWeight", Condition.HttpWeight);
            Caches.MongoDbCacheUpdate<ServerCondition>(t => t.Key == Condition.Key, "TcpWeight", Condition.TcpWeight);
            return await Task.FromResult("添加成功!");
        }
    }
}
