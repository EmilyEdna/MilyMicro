using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Service.CenterApi.ViewModel;
using Mily.Service.CenterRpc.RpcSetting.Handler;
using Mily.Service.CenterRpc.RpcSetting.Result;
using Mily.Service.ReplyApi.ProxyFilter;
using Mily.Service.ViewSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.Common;
using XExten.XCore;

namespace Mily.Service.ReplyApi
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "/Proxy")]
    [ProxyApiFilter]
    public class ProxyApi
    {
        [Post]
        [JsonDataConvert]
        public object ProxyServcie(IHttpContext Context)
        {
            var Request = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<Dictionary<String, Object>>();
            if (Request == null)
                Request = new Dictionary<String, Object>();
            Configuration.Heads.ToEachs(Item =>
            {
                if (Item.Key != "Server")
                    Request.Add(Item.Key,Item.Value);
            });
            ServerCondition Condition = Caches.MongoDBCacheGet<ServerCondition>(t => t.ServiceName== Configuration.Heads["Server"].ToString());
            var Event = EventCache.GetPacketCache(Condition.ServiceName);
            ServerKey Key = ServerKey.SetValue(NetTypeEnum.Listened, Condition.ServiceName);
            var NewEvent = Event.SetInfo(Event.Session, ResultProvider.SetValue(Key, Request));
            Event.Session.Server.Handler.SessionPacketDecodeCompleted(Event.Server, NewEvent);
            return ResultEvent.StaticResult;
        }
    }
}
