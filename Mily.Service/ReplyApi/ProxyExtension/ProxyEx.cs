using Mily.Service.CenterApi.ViewModel;
using Mily.Service.CenterRpc.RpcSetting.Handler;
using Mily.Service.CenterRpc.RpcSetting.Result;
using Mily.Service.ViewSetting;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.CacheFactory;
using XExten.Common;
using XExten.HttpFactory;
using XExten.XCore;
using System.Linq;

namespace Mily.Service.ReplyApi.ProxyExtension
{
    public class ProxyEx
    {
        private static Object TCP(Dictionary<String, Object> Request)
        {
            Request ??= new Dictionary<String, Object>();
            Request.Add("Method", RouteConfiger.Method);
            Request.Add("DataBase", Configuration.Heads.DataBase);
            ServerCondition Condition = Caches.MongoDBCacheGet<ServerCondition>(t => t.ServiceName == RouteConfiger.Server && t.Stutas == 1);
            var Event = EventCache.GetPacketCache(Condition.ServiceName);
            ServerKey Key = ServerKey.SetValue(NetTypeEnum.Listened, Condition.ServiceName);
            var NewEvent = Event.SetInfo(Event.Session, ResultProvider.SetValue(Key, Request));
            Event.Session.Server.Handler.SessionPacketDecodeCompleted(Event.Server, NewEvent);
            return ResultEvent.StaticResult;
        }
        private static Object Http(Dictionary<String, Object> Request)
        {
            ServerCondition Condition = Caches.MongoDBCacheGet<ServerCondition>(t => t.ServiceName == RouteConfiger.Server && t.Stutas == 1);
            String Path = $"http://{ Condition.Host}:{Condition.HttpPort}/Api/{RouteConfiger.Controllor}/{RouteConfiger.Method}";
            List<KeyValuePair<String, String>> Param = new List<KeyValuePair<String, String>>();
            foreach (var item in Request)
            {
                Param.Add(new KeyValuePair<String, String>(item.Key,item.Value.ToString()));
            }
            return HttpMultiClient.HttpMulti.Headers("ActionType", Configuration.Heads.DataBase.ToString())
                   .Header("Global", (Request.ContainsKey("Global") ? Request["Request"].ToString() : null))
                   .AddNode(Path, Param, RequestType.POST)
                   .Build().RunString();
        }
        /// <summary>
        /// 负载均衡
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static Object LoadBalance(Dictionary<String, Object> Request)
        {
            ServerCondition Condition = Caches.MongoDBCacheGet<ServerCondition>(t => t.ServiceName == RouteConfiger.Server && t.Stutas == 1);
            int Seeds = (new Random(Guid.NewGuid().GetHashCode())).Next(1, 10);
            if (!Condition.UseHttp)
                return TCP(Request);
            else
            {
                if (Condition.HttpWeight.IsNullOrEmpty() && Condition.TcpWeight.IsNullOrEmpty())
                    return TCP(Request);
                else if (Condition.HttpWeight.IsNullOrEmpty() && !Condition.TcpWeight.IsNullOrEmpty())
                    return TCP(Request);
                else if (!Condition.HttpWeight.IsNullOrEmpty() && Condition.TcpWeight.IsNullOrEmpty())
                    return Http(Request);
                else
                {
                    if (Condition.TcpWeight.Split(",").ToList().Min(t => Convert.ToInt32(t)) <= Seeds && Condition.TcpWeight.Split(",").ToList().Max(t => Convert.ToInt32(t)) >= Seeds)
                        return TCP(Request);
                    else
                        return Http(Request);
                }
            }
        }
    }
}
