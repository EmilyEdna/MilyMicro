using Mily.ViewModels.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.ViewModels
{
    /// <summary>
    /// 功能菜单
    /// </summary>
    public class RoleMenuRouterViewModel : BasicViewModel
    {
        /// <summary>
        /// 菜单表Id
        /// </summary>
        public Guid? MenuItemId { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string FuncName { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; }

        /// <summary>
        /// 功能路由
        /// </summary>
        public string FuncRouterPath { get; set; }
    }
}
