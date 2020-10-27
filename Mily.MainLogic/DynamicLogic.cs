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
    }
}
