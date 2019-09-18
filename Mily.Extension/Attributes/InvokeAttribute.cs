using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mily.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class InvokeAttribute:Attribute
    {
        public List<String> Name { get; set; }
        public InvokeAttribute(params String[] Param)
        {
            Name = Param.ToList();
        }
        /// <summary>
        /// 判断角色
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public bool JudgeRoles(List<String> Param) {
            return false;
        }
    }
}
