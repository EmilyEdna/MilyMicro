using Castle.DynamicProxy;
using Mily.Extension.Infrastructure.Common;
using Mily.Setting;
using Mily.Setting.DbTypes;
using Polly;
using System;
using System.Linq;
using System.Threading.Tasks;
using XExten.Common;

namespace Mily.DbCore
{
    public class ServiceProxy : ExcuteProxy, IInterceptor
    {
        public void Intercept(IInvocation Invocation)
        {
            StarExcute();
            SugerDbContext.TypeAttrbuite = MilyConfig.DbTypeAttribute;
            Invocation.ReturnValue = OnExcute(Invocation).GetAwaiter().GetResult();
            EndExcute();
        }

        public override async Task<object> OnExcute(dynamic Dynamic)
        {
            IInvocation Invocation = (IInvocation)Dynamic;
            Object Result = Invocation.Method.Invoke(Invocation.InvocationTarget, Invocation.Arguments);
            return await Task.FromResult(Result);
        }
    }
}
