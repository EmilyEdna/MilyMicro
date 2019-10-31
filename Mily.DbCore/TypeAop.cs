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
            //不是系统参数是时候
            if (Invocation.Arguments.Any(item => !item.GetType().Namespace.ToUpper().Contains("SYSTEM")))
            {
                Object TargetValue = Invocation.Arguments.Where(item => !item.GetType().Namespace.ToUpper().Contains("SYSTEM")).FirstOrDefault();
                if (TargetValue.GetType() == typeof(PageQuery)) //参数为分页参数类型时
                    SugerDbContext.TypeAttrbuite = (TargetValue as PageQuery).KeyWord.ContainsKey("DbTypeAttribute") ? (DBType)Convert.ToInt32((TargetValue as PageQuery).KeyWord["DbTypeAttribute"]) : DBType.Default;
                else if (TargetValue.GetType() == typeof(DBType)) //参数为数据库类型时
                    SugerDbContext.TypeAttrbuite = (DBType)TargetValue;
                else //参数为实体类型时
                    SugerDbContext.TypeAttrbuite = (TargetValue.GetType().GetProperty("DbTypeAttribute").GetValue(TargetValue) == null ?
                        DBType.Default : (DBType)TargetValue.GetType().GetProperty("DbTypeAttribute").GetValue(TargetValue));
            }//系统参数的时候
            else
                SugerDbContext.TypeAttrbuite = MilyConfig.DbTypeAttribute;
            Invocation.ReturnValue = Invocation.Method.Invoke(Invocation.InvocationTarget, Invocation.Arguments);
        }
    }
}
