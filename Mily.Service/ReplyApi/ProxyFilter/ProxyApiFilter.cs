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
            Configuration.Heads = context.HttpContext.Request.Header["Author"].IsNullOrEmpty() ? new HeadConfiger() : context.HttpContext.Request.Header["Author"].ToLzStringDec().ToModel<HeadConfiger>();
            return base.Executing(context);
        }
    }
}
