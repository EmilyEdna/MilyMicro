using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using WebApiClientCore.Extensions.OAuths;
using XExten.XCore;
using XExten.CacheFactory;
using Mily.Setting;

namespace Mily.WebClient
{
    public class MilyTokenAttribute : ApiFilterAttribute
    {
        public override async Task OnRequestAsync(ApiRequestContext context)
        {
            if (context.ApiAction.Name != "Login")
            {
                WebTokenViewModel WebToken = Caches.RunTimeCacheGet<WebTokenViewModel>(MilyConfig.CacheKey);
                context.HttpContext.RequestMessage.Headers.Add("Global", WebToken?.Global);
                context.HttpContext.RequestMessage.Headers.Add("Authorization", WebToken?.JWTToken);
                context.HttpContext.RequestMessage.Headers.Add("ActionType", "1");
            }
            await Task.CompletedTask;
        }

        public override async Task OnResponseAsync(ApiResponseContext context)
        {
            if (context.ApiAction.Name == "Login")
            {
                var Result = context.Result.ToString().ToModel<JObject>();
                WebTokenViewModel WebToken = new WebTokenViewModel
                {
                    JWTToken = Result.SelectToken("ResultData.AuthorToken").ToString(),
                    Global = Result.SelectToken("ResultData.Data.KeyId").ToString()
                };
                Caches.RunTimeCacheSet(MilyConfig.CacheKey, WebToken,120);
            }
            await Task.CompletedTask;
        }
    }
}
