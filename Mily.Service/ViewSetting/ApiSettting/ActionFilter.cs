using BeetleX.FastHttpApi;
using XExten.XCore;

namespace Mily.Service.ViewSetting.ApiSettting
{
    public class ActionFilter : FilterAttribute
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