using BeetleX.FastHttpApi;
using Mily.Gateway.GatewayCenter.NetCenter.FilterGroup;
using Mily.Gateway.GatewaySetting.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.XCore;

namespace Mily.Gateway.GatewayCenter.NetCenter
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "/Center")]
    [NetCenterFilter]
    public class NetServer
    {
        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        [Post]
        public async Task<String> InsertRoute(IHttpContext Context)
        {
            ServerCondition Conditions = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<ServerCondition>();
            Caches.MongoDbCacheUpdate<ServerCondition>(t => t.Key == Conditions.Key, "Route", Conditions.Route);
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
            var Condition = Caches.MongoDBCacheGet<ServerCondition>(t => t.Key == Conditions.Key);
            if (Condition.HttpPort.IsNullOrEmpty())
                return await Task.FromResult("请先添加HTTP端口号!");
            else
                Caches.MongoDbCacheUpdate<ServerCondition>(t => t.Key == Conditions.Key, "UseHttp", Conditions.UseHttp.ToString());
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
        /// <summary>
        /// 获取分组服务
        /// </summary>
        /// <returns></returns>
        [Get]
        public async Task<Object> GetGroupServer()
        {
            var data = Caches.MongoDBCachesGet<ServerCondition>(t => true).GroupBy(t => t.ServiceName).Select(t => new ServerGroupCondition
            {
                ServiceName = t.Key,
                Conditions = t.ToList()
            });
            return await Task.FromResult(data);
        }
    }
}
