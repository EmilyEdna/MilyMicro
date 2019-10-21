using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mily.Setting.ModelEnum
{
    /// <summary>
    /// 等级枚举
    /// </summary>
    public enum MenuItemEnum
    {
        /// <summary>
        /// 一级
        /// </summary>
        [Description("一级")]
        Lv1 = 1,
        /// <summary>
        /// 二级
        /// </summary>
        [Description("二级")]
        Lv2 = 2,
        /// <summary>
        /// 三级
        /// </summary>
        [Description("三级")]
        Lv = 3
    }
}
