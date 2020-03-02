using Microsoft.AspNetCore.Http;
using Mily.Setting;
using System;
using System.Threading.Tasks;
using XExten.XCore;

namespace Mily.Extension.Infrastructure.GeneralMiddleWare
{
    public class ResultMiddleWare
    {
        private readonly RequestDelegate RequestDelegate;
        public ResultMiddleWare(RequestDelegate RequestDelegates)
        {
            RequestDelegate = RequestDelegates;
        }
        public async Task Invoke(HttpContext Context)
        {
            if (!Context.Request.Path.Value.Contains("Login"))
                MilyConfig.CacheKey = Context.Request.Headers["Global"].ToString().IsNullOrEmpty() ? String.Empty : Context.Request.Headers["Global"].ToString().ToLzStringDec();
            Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            await RequestDelegate(Context);
        }
    }
}
