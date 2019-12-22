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
            Configuration.Heads = (Dictionary<String, Object>)context.HttpContext.Request.Header["Author"].ToLzStringDec().ToModel<HeadConfiger>().ToDic();
            return base.Executing(context);
        }
    }
}
