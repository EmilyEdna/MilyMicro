using BeetleX.FastHttpApi;
using Mily.Service.ViewSetting;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace Mily.Service.ReplyApi.ProxyFilter
{
    public class ProxyApiFilter : FilterAttribute
    {
        public override bool Executing(ActionContext context)
        {
            var Author = context.HttpContext.Request.Header["Author"];
            var Authorization = context.HttpContext.Request.Header["Authorization"];
            Configuration.Heads = Author.IsNullOrEmpty() ? new HeadConfiger() : Author.ToLzStringDec().ToModel<HeadConfiger>();
            Configuration.Authorization = Authorization.IsNullOrEmpty() ? "" : Authorization;
            return base.Executing(context);
        }
    }
}
