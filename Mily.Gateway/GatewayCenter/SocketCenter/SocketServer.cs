using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Gateway.GatewayCenter.SocketCenter.Balance;
using Mily.Gateway.GatewayCenter.SocketCenter.FilterGroup;
using Mily.Gateway.GatewaySetting;
using Mily.Gateway.GatewaySetting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using XExten.CacheFactory;
using XExten.XCore;

namespace Mily.Gateway.GatewayCenter.SocketCenter
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "/Proxy")]
    [SocketFilter]
    public class SocketServer: IController
    {
        [NotAction]
        public void Init(HttpApiServer server, string path)
        {
            server.HttpRequesting += (Obj, Event) =>
            {
                foreach (var Item in Configuration.FuncArray)
                {
                    if (Event.Request.BaseUrl.Contains(Item))
                    {
                        List<string> UrlList = Event.Request.BaseUrl.Split("/").ToList();
                        UrlList.Remove("");
                        RouteConfiger.Server = UrlList[1];
                        RouteConfiger.Controllor = UrlList[2];
                        RouteConfiger.Method = UrlList[3];
                        string Route = Caches.MongoDBCacheGet<ServerCondition>(t => t.ServiceName == RouteConfiger.Server && t.Stutas == 1).Route;
                        Event.Request.UrlRewriteTo(Route.IsNullOrEmpty() ? $"/Proxy/{Item}" : $"/Proxy/{Route}");
                        return;
                    }
                }
            };
        }

        [Post]
        [JsonDataConvert]
        public object ProxyMain(IHttpContext Context)
        {
            Dictionary<String, Object> Request = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<Dictionary<String, Object>>();
            return SocketBalance.LoadBalance(Request);
        }
        [Post]
        [JsonDataConvert]
        public object ProxyHttp(IHttpContext Context) {
            Dictionary<String, Object> Request = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<Dictionary<String, Object>>();
            return SocketBalance.Http(Request);
        }
        [Post]
        [JsonDataConvert]
        public object ProxyTcp(IHttpContext Context)
        {
            Dictionary<String, Object> Request = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<Dictionary<String, Object>>();
            return SocketBalance.TCP(Request);
        }
    }
}
