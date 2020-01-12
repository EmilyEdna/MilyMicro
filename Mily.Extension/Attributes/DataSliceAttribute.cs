using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mily.Extension.Attributes
{
    /// <summary>
    /// 指定数据库
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DataSliceAttribute : Attribute
    {
        public List<String> DBName { get; set; }
        public DataSliceAttribute(params String[] DBNames)
        {
            DBName = DBNames.ToList();
        }
    }
}
