using Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.Handle;
using Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.View;
using Mily.Gateway.GatewayEvent.SocketEvent;
using Mily.Gateway.GatewaySetting;
using Mily.Gateway.GatewaySetting.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.CacheFactory;
using XExten.Common;
using XExten.HttpFactory;
using XExten.XCore;
using XExten.XPlus;

namespace Mily.Gateway.GatewayCenter.SocketCenter.Balance
{
    public class SocketBalance
    {
        public static Object TCP(Dictionary<String, Object> Request)
        {
            return XPlusEx.XTry<Object>(() =>
            {
                Request ??= new Dictionary<String, Object>();
                Request.Add("Method", RouteConfiger.Method);
                Request.Add("DataBase", Configuration.Heads.DataBase);
                Request.Add("Authorization", Configuration.Authorization);
                ServerCondition Condition = Caches.MongoDBCacheGet<ServerCondition>(t => t.ServiceName == RouteConfiger.Server && t.Stutas == 1);
                var Event = SocketCache.GetPacketCache(Condition.ServiceName);
                ServerKey Key = ServerKey.SetValue(SocketTypeEnum.Listened, Condition.ServiceName);
                var NewEvent = Event.SetInfo(Event.Session, ResultProvider.SetValue(Key, Request));
                Event.Session.Server.Handler.SessionPacketDecodeCompleted(Event.Server, NewEvent);
                return EventAction.Instance().DelegateResult;
            }, (Ex) => { return Http(Request); });
        }
        public static Object Http(Dictionary<String, Object> Request)
        {
            return XPlusEx.XTry<Object>(() =>
            {
                ServerCondition Condition = Caches.MongoDBCacheGet<ServerCondition>(t => t.ServiceName == RouteConfiger.Server && t.Stutas == 1);
                String Path = $"http://{ Condition.Host}:{Condition.HttpPort}/Api/{RouteConfiger.Controllor}/{RouteConfiger.Method}";
                List<KeyValuePair<String, String>> Param = new List<KeyValuePair<String, String>>();
                foreach (var item in Request)
                {
                    if (item.Value is JObject)
                        item.Value.ToJson().ToModel<Dictionary<String, String>>().ToEachs(Selector =>
                        {
                            Param.Add(new KeyValuePair<String, String>($"{item.Key}[{Selector.Key}]", Selector.Value));
                        });
                    else
                        Param.Add(new KeyValuePair<String, String>(item.Key, item.Value.ToString()));
                }
                var Header = HttpMultiClient.HttpMulti.Headers("ActionType", Configuration.Heads.DataBase.ToString())
                        .Header("Global", (Request.ContainsKey("Global") ? Request["Global"].ToString().ToLzStringDec() : null));
                if (!Configuration.Authorization.IsNullOrEmpty())
                    Header.Header("Authorization", Configuration.Authorization);
                return Header.AddNode(Path, Param, Configuration.Heads.Method)
                         .Build().RunString().FirstOrDefault().ToModel<Object>();
            }, (Ex) => { return TCP(Request); });
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
            if (Condition == null) return Http(Request);
            if (!Condition.UseHttp) return TCP(Request);
            else
            {
                if (Condition.HttpWeight.IsNullOrEmpty() && Condition.TcpWeight.IsNullOrEmpty()) return TCP(Request);
                else if (Condition.HttpWeight.IsNullOrEmpty() && !Condition.TcpWeight.IsNullOrEmpty()) return TCP(Request);
                else if (!Condition.HttpWeight.IsNullOrEmpty() && Condition.TcpWeight.IsNullOrEmpty()) return Http(Request);
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
