using Mily.ViewModels.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.ViewModels
{
    /// <summary>
    /// 菜单路由
    /// </summary>
    public class RoleMenuRouterViewModel : BasicViewModel
    {
        /// <summary>
        /// 菜单表Id
        /// </summary>
        public int? MenuItemId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string MenuPath { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string RouterPath { get; set; }

        /// <summary>
        /// 子菜单路由
        /// </summary>
        public List<RoleMenuRouterViewModel> ChildFeatures { get; set; }
    }
}
