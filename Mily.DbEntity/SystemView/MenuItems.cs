using Mily.Extension.Attributes;
using Mily.Setting.ModelEnum;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Mily.DbEntity.SystemView
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [SugarTable("System_MenuItems", "系统菜单表")]
    [DataSlice("Test")]
    public class MenuItems : BaseEntity
    {
        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "图标", Length = 50)]
        public string Icon { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "菜单名称", Length = 50)]
        public string Title { get; set; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "菜单地址", Length = 50)]
        public string Path { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "路由地址", Length = 50)]
        public string RouterPath { get; set; }

        /// <summary>
        /// 菜单级别
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "INT", ColumnDescription = "菜单级别")]
        public MenuItemEnum? Lv { get; set; }

        /// <summary>
        /// 是否父菜单
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "BIT", ColumnDescription = "是否父菜单")]
        public bool? Parent { get; set; }

        /// <summary>
        /// 父菜单标识
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "父菜单")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<MenuItems> ChildMenus { get; set; }
    }
}