using Mily.Setting.ModelEnum;
using Mily.ViewModels.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.ViewModels
{
    /// <summary>
    /// 菜单角色
    /// </summary>
    public class RoleMenuItemViewModel : BasicViewModel
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单级别
        /// </summary>
        public MenuItemEnum? Lv { get; set; }

        /// <summary>
        /// 是否父菜单
        /// </summary>
        public bool? Parent { get; set; }

        /// <summary>
        /// 父菜单标识
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<RoleMenuItemViewModel> ChildMenus { get; set; }
    }
}
