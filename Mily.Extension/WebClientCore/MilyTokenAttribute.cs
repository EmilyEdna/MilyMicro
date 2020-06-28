using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mily.Extension.AutofacIoc;
using Mily.Extension.LoggerFactory;
using Mily.Extension.WebClientCore.MainApi;
using Mily.Setting;
using Newtonsoft.Json.Linq;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using XExten.CacheFactory;
using XExten.XCore;
using System.Linq;

namespace Mily.Extension.WebClientCore
{
    public class MilyTokenAttribute : ApiFilterAttribute
    {
        public override async Task OnRequestAsync(ApiRequestContext context)
        {
            String WebToken = Caches.RedisCacheGet<String>(MilyConfig.MicroKey);
            context.HttpContext.RequestMessage.Headers.Add("Global", MilyConfig.MicroKey.Split("_")[0]);
            context.HttpContext.RequestMessage.Headers.Add("Authorization", WebToken);
            context.HttpContext.RequestMessage.Headers.Add("ActionType", "1");
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
