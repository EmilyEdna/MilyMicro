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
            Object TargetValue = Invocation.Arguments.Where(item => !item.GetType().Namespace.ToUpper().Contains("SYSTEM")).FirstOrDefault();
            Int32 ParamCount = Invocation.Arguments.Count();
            if (ParamCount > 0 && ParamCount <= 1)
            {
                if (TargetValue.GetType() == typeof(PageQuery))
                    SugerDbContext.TypeAttrbuite = (TargetValue as PageQuery).KeyWord.ContainsKey("DbTypeAttribute") ? (DBType)Convert.ToInt32((TargetValue as PageQuery).KeyWord["DbTypeAttribute"]) : DBType.Default;
                else if (TargetValue.GetType() == typeof(DBType))
                    SugerDbContext.TypeAttrbuite = (DBType)TargetValue;
                else
                    SugerDbContext.TypeAttrbuite = (TargetValue.GetType().GetProperty("DbTypeAttribute").GetValue(TargetValue) == null ?
                        DBType.Default : (DBType)TargetValue.GetType().GetProperty("DbTypeAttribute").GetValue(TargetValue));
            }
            else if (ParamCount > 1)
                SugerDbContext.TypeAttrbuite = (DBType)Invocation.Arguments.Where(item => item.GetType() == typeof(DBType)).FirstOrDefault();
            else
                SugerDbContext.TypeAttrbuite = MilyConfig.DbTypeAttribute;

            Invocation.ReturnValue = Invocation.Method.Invoke(Invocation.InvocationTarget, Invocation.Arguments);
        }
    }
}
