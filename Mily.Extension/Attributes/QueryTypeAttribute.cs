using System;
using System.Collections.Generic;
using System.Text;
using XExten.XExpres;

namespace Mily.Extension.Attributes
{
    /// <summary>
    /// 检索字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class QueryTypeAttribute : Attribute
    {
        /// <summary>
        /// 检索方式
        /// </summary>
        public QType SearchType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string KeyWord { get; set; }

        public QueryTypeAttribute(string Key)
        {
            KeyWord = Key;
        }

        public QueryTypeAttribute(QType Qype,string Key)
        {
            SearchType = Qype;
            KeyWord = Key;
        }
    }
}
