using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Service.CenterApi.ViewModel;
using Mily.Service.CenterRpc.RpcSetting.Handler;
using Mily.Service.CenterRpc.RpcSetting.Result;
using Mily.Service.ReplyApi.ProxyExtension;
using Mily.Service.ReplyApi.ProxyFilter;
using Mily.Service.ViewSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.Common;
using XExten.XCore;

namespace Mily.Service.ReplyApi
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "/Proxy")]
    [ProxyApiFilter]
    public class ProxyApi : IController
    {
        [NotAction]
        public void Init(HttpApiServer server, string path)
        {
            server.HttpRequesting += (Obj, Event) =>
            {
                Configuration.FuncArray.ForEach(Item =>
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
                    }
                });
            };
        }

        [Post]
        [JsonDataConvert]
        public object ProxyMain(IHttpContext Context)
        {
            Dictionary<String, Object> Request = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<Dictionary<String, Object>>();
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
    }
}
