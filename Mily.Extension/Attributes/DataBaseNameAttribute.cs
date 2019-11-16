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
    public class DataBaseNameAttribute : Attribute
    {
        public List<String> DbHostAttr { get; set; }
        public DataBaseNameAttribute(params String[] HostAttr)
        {
            DbHostAttr = HostAttr.ToList();
        }
    }
}
