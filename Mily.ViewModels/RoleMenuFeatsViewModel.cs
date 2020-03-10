using Mily.ViewModels.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.ViewModels
{
    /// <summary>
    /// 菜单功能
    /// </summary>
    public class RoleMenuFeatsViewModel:BasicViewModel
    {
        /// <summary>
        /// 菜单表ID
        /// </summary>
        public Guid? MenuItemId { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string FeatName { get; set; }

        /// <summary>
        /// 功能图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 启用或禁用
        /// </summary>
        public bool? EnableOrDisable { get; set; }
    }
}
