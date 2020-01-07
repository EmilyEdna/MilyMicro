using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace Mily.Extension.Infrastructure.Common
{
    /// <summary>
    /// 获取Attribute值
    /// </summary>
    public static class AttributeUnity
    {
        /// <summary>
        /// 获取DescriptionAttribute值
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public static string GetDescriptionValue(this Enum Param)
        {
            return (Param.GetType().GetField(Param.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute).Description;
        }

        /// <summary>
        /// 获取特定的Attribute
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="Param"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static TAttribute GetTargetAttributeMethod<TSource, TAttribute>(this TSource Param, string FieldName) where TAttribute : Attribute
        {
            return (Param.GetType().GetField(FieldName).GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute);
        }
    }
}
