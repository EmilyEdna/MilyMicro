using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbCore
{
    public class TypeAop : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            SugerDbContext.TypeAttrbuite = DBType.MSSQL;
            invocation.ReturnValue = invocation.Method.Invoke(invocation.InvocationTarget, invocation.Arguments);
        }
    }
}
