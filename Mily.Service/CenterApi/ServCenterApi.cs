using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Service.CenterApi.CenterAop;
using Mily.Service.CenterApi.ViewModel;
using Mily.Service.CenterRpc.RpcSetting.Handler;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.Common;
using XExten.XCore;
using System.Collections.Generic;

namespace Mily.Service.CenterApi
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "/Center")]
    [CenterFilter]
    [DefaultJsonResultFilter]
    public class ServCenterApi
    {
        [Post]
        [JsonDataConvert]
        public void insert(IHttpContext Context)
        {
            //ServerCondition Condition = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<ServerCondition>();
            //Caches.MongoDBCacheSet(Condition);
            var Event = Caches.MongoDBCacheGet<PacketDecodeCompletedEventArgs>(t => true);
            var NewEvent = Event.SetInfo(Event.Session, ResultProvider.SetValue(NetTypeEnum.Listened, new Dictionary<object, object> { { "我是", "1" } }));
            Event.Session.Server.Handler.SessionPacketDecodeCompleted(Event.Server, NewEvent);
        }
        [Get]
        public async Task<Object> GetServer()
        {
            return await Task.Run(() =>
           {
               return Caches.MongoDBCachesGet<ServerCondition>(t => true);
           });
        }
    }
}
