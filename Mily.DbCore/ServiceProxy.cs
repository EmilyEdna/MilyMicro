using Castle.DynamicProxy;
using Mily.Extension.Infrastructure.Common;
using Mily.Setting;
using System;
using System.Threading.Tasks;
using Mily.Extension.Retry;
using System.Linq;

namespace Mily.DbCore
{
    public class ServiceProxy : ExcuteProxy, IInterceptor
    {
        public void Intercept(IInvocation Invocation)
        {
            StarExcute();
            SugerDbContext.TypeAttrbuite = MilyConfig.DbTypeAttribute;
            Invocation.ReturnValue = RetryException.DoRetry(() => OnExcute(Invocation));
            EndExcute();
        }
        public override Object OnExcute(dynamic Dynamic)
        {
            IInvocation Invocation = (IInvocation)Dynamic;
            dynamic Result = Invocation.Method.Invoke(Invocation.InvocationTarget, Invocation.Arguments);
            if (Result.Exception != null)
            {
                if (Result.Exception.Count != null)
                    return Result.Exception.FirstOrDefault();
                else
                    return Result;
            }
            else
                return Result;
        }
    }
}
