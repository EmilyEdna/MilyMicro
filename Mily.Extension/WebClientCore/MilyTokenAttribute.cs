using System;
using System.Threading.Tasks;
using Mily.Extension.LoggerFactory;
using Mily.Setting;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using XExten.CacheFactory;
using XExten.XCore;

namespace Mily.Extension.WebClientCore
{
    public class MilyTokenAttribute : ApiFilterAttribute
    {
        public override async Task OnRequestAsync(ApiRequestContext context)
        {
            //不需要登录的接口调用内部的接口也必须是不需要登录授权的接口
            if (!MilyConfig.MicroKey.IsNullOrEmpty())
            {
                String WebToken = Caches.RedisCacheGet<String>(MilyConfig.MicroKey);
                context.HttpContext.RequestMessage.Headers.Add("Global", MilyConfig.MicroKey.Split("_")[0]);
                context.HttpContext.RequestMessage.Headers.Add("Authorization", WebToken);
            }
            context.HttpContext.RequestMessage.Headers.Add("ActionType", MilyConfig.DbTypeAttribute.GetHashCode().ToString());
            await Task.CompletedTask;
        }

        public async override Task OnResponseAsync(ApiResponseContext context)
        {
            if (context.Exception != null)
            {
                LogFactoryExtension.WriteError(typeof(MilyTokenAttribute).FullName, context.ApiAction.Name, context.Arguments.ToJson(),context.Exception.Message, context.HttpContext.RequestMessage.RequestUri.AbsolutePath);
                throw context.Exception;
            }
            await Task.CompletedTask;
        }
    }
}
