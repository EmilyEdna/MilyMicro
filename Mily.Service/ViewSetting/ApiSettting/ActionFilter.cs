using BeetleX.FastHttpApi;
using Newtonsoft.Json.Linq;
using System;
using XExten.XCore;

namespace Mily.Service.ViewSetting.ApiSettting
{
    public class ActionFilter : FilterAttribute
    {
        public override bool Executing(ActionContext context)
        {
            IHttpContext HttpContext = context.HttpContext;
            return base.Executing(context);
        }

        public override void Executed(ActionContext context)
        {
            IHttpContext HttpContext = context.HttpContext;
            String Key = HttpContext.Request.Header["Cross"].ToLzStringDec();
            if (Key.Equals("Mily")) 
            {
                ResultMiddleWare Middle = context.Result.ToString().ToModel<ResultMiddleWare>();
                String Cookie = JToken.FromObject(Middle.ResultData).SelectToken("KeyId").ToObject<String>();
                HttpContext.Response.SetCookie("Global", Cookie, "/", DateTime.Now.AddHours(2));
                HttpContext.Response.Header["Access-Control-Allow-Credentials"] = "true";
            }
            //base.Executed(context);
        }
    }
}