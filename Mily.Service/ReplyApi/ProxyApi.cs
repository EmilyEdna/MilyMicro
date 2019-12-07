﻿using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Service.CenterApi.ViewModel;
using Mily.Service.CenterRpc.RpcSetting.Handler;
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
    [DefaultJsonResultFilter]
    public class ProxyApi
    {
        [Post]
        [JsonDataConvert]
        public void TestProxy(IHttpContext Context)
        {
            var Request = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<Dictionary<string, Object>>();
            ServerCondition Condition = Caches.MongoDBCacheGet<ServerCondition>(t => t.Stutas != 0);
            var Event = EventCache.GetPacketCache(Condition.ServiceName);
            ServerKey Key = ServerKey.SetValue(NetTypeEnum.Listened, Condition.ServiceName);
            var NewEvent = Event.SetInfo(Event.Session, ResultProvider.SetValue(Key, Request));
            Event.Session.Server.Handler.SessionPacketDecodeCompleted(Event.Server, NewEvent);
        }
    }
}
