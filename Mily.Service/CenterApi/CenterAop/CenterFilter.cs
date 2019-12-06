using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.CenterApi.CenterAop
{
    /// <summary>
    /// 服务中心过滤器
    /// </summary>
   public class CenterFilter: FilterAttribute
    {
        public override bool Executing(ActionContext context)
        {
            return base.Executing(context);
        }
    }
}
