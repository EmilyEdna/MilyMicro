using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.ViewSetting.ApiSettting
{
    public class ActionFilter: FilterAttribute
    {
        public override bool Executing(ActionContext context)
        {
            return base.Executing(context);
        }
        public override void Executed(ActionContext context)
        {
            base.Executed(context);
        }
    }
}
