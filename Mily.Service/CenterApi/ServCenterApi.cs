using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using System.Linq;
using XExten.CacheFactory;
using System;
using System.Collections.Generic;
using System.Text;
using Mily.Service.CenterApi.ViewModel;
using XExten.XCore;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Mily.Service.CenterApi
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "/center")]
    public class ServCenterApi
    {
        [Post]
        [JsonDataConvert]
        public void insert(IHttpContext Context)
        {
            ServerCondition Condition = Context.Data.Copy().FirstOrDefault().Value.ToJson().ToModel<ServerCondition>();
            Caches.MongoDBCacheSet(Condition);
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
