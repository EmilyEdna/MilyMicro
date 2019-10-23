using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbCore
{
    public class TypeAop : IInterceptor
    {
        public void Intercept(IInvocation Invocation)
        {
            SugerDbContext.TypeAttrbuite = DBType.MSSQL;
            Invocation.ReturnValue = Invocation.Method.Invoke(Invocation.InvocationTarget, Invocation.Arguments);
        }
    }
}
