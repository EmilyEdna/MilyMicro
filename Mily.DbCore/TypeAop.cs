using Castle.DynamicProxy;
using Mily.Setting;
using Mily.Setting.DbTypes;
using System;
using System.Linq;
using XExten.Common;

namespace Mily.DbCore
{
    public class TypeAop : IInterceptor
    {
        public void Intercept(IInvocation Invocation)
        {
            SugerDbContext.TypeAttrbuite = MilyConfig.DbTypeAttribute;
            Invocation.ReturnValue = Invocation.Method.Invoke(Invocation.InvocationTarget, Invocation.Arguments);
        }
    }
}
