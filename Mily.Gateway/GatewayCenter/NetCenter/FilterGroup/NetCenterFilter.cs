using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Gateway.GatewayCenter.NetCenter.FilterGroup
{
    /// <summary>
    /// 服务中心过滤器
    /// </summary>
    public class NetCenterFilter: FilterAttribute
    {
        public override bool Executing(ActionContext context)
        {
            return base.Executing(context);
        }
    }
}
