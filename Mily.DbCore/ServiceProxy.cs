﻿using Castle.DynamicProxy;
using Mily.Extension.Infrastructure.Common;
using Mily.Extension.Retry;
using Mily.Setting;
using Newtonsoft.Json;
using System;
using System.Linq;
using XExten.Profile.AspNetCore.InvokeTracing;
using XExten.XCore;

namespace Mily.DbCore
{
    public class ServiceProxy : ExcuteProxy, IInterceptor
    {
        public void Intercept(IInvocation Invocation)
        {
            StarExcute(() => Console.Write($"\n执行方法：{Invocation.InvocationTarget.GetType().FullName}.{Invocation.Method.Name}，执行参数：{Invocation.Arguments.FirstOrDefault().ToJson()}，"));
            SugerDbContext.TypeAttrbuite = MilyConfig.DbTypeAttribute;
            Invocation.ReturnValue = RetryException.DoRetry(() => OnExcute(Invocation));
            EndExcute(() => Console.Write($"\n执行结果：{JsonConvert.SerializeObject((Invocation.ReturnValue as dynamic).Result)}，"));
        }
        public override Object OnExcute(dynamic Dynamic)
        {
            IInvocation Invocation = (IInvocation)Dynamic;
            dynamic Result = Invocation.Method.ByTraceInvoke(Invocation.InvocationTarget, Invocation.Arguments);
            if (Result.Exception != null)
            {
                if (Result.Exception.GetType().Name.Contains("Exception"))
                    return Result;
                else
                    return Result.Exception.FirstOrDefault();
            }
            else
                return Result;
        }
    }
}
