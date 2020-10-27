using Mily.DbCore;
using Mily.Extension.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using XExten.Common;
using XExten.XCore;
using XExten.XExpres;
using XExten.XPlus;

namespace Mily.MainLogic
{
    public static class DynamicLogic
    {

        private static QueryTypeAttribute GetQueryType<T>(string PropertyName)
        {
            return (typeof(T).GetProperty(PropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)?
                .GetCustomAttributes(typeof(QueryTypeAttribute), false).FirstOrDefault() as QueryTypeAttribute);
        }

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Param"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        public static object GetField(string Param, PageQuery Query)
        {
            return Query.KeyWord.Keys.Contains(Param.ToLower()) ? Query.KeyWord[Param.ToLower()] : null;
        }

        /// <summary>
        /// 批量生成WhereIF
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sugar"></param>
        /// <returns></returns>
        public static ISugarQueryable<T> AnalysisIF<T>(this ISugarQueryable<T> Sugar, PageQuery Query)
        {

            Query.KeyWord.Keys.ToEachs(Item =>
            {
                QueryTypeAttribute Target = GetQueryType<T>(Item);
                if (Target != null)
                    Sugar = Sugar.WhereIF(!Query.KeyWord[Item].IsNullOrEmpty(), XExp.GetExpression<T>(Target.KeyWord, Query.KeyWord[Item], Target.SearchType));
            });
            return Sugar;
        }
    }
}
