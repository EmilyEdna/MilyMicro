using BeetleX.FastHttpApi;
using Mily.Gateway.GatewaySetting;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace Mily.Gateway.GatewayCenter.SocketCenter.FilterGroup
{
    public class SocketFilter: FilterAttribute
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
