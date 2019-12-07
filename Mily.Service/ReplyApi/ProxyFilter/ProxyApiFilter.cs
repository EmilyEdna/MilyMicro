using BeetleX.FastHttpApi;
using Mily.Service.ViewSetting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.ReplyApi.ProxyFilter
{
    public class ProxyApiFilter : FilterAttribute
    {
        public override bool Executing(ActionContext context)
        {
            Configuration.ServiceName = context.HttpContext.Request.Header["Server"];
            return base.Executing(context);
        }
    }
}
