using Mily.Extension.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbEntity.SystemView.MenuSeries
{
    /// <summary>
    /// 系统菜单功能表
    /// </summary>
    [SugarTable("Sys_MenuFeatures", "系统菜单功能表")]
    [DataSlice("Test")]
    public class MenuFeatures: BaseEntity
    {
        /// <summary>
        /// 菜单表ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "菜单表ID")]
        public int? MenuItemId { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "功能名称", Length = 50)]
        public string FeatName { get; set; }

        /// <summary>
        /// 功能图标
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "功能图标", Length = 50)]
        public string Icon { get; set; }

        /// <summary>
        /// 启用或禁用
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "BIT", ColumnDescription = "启用或禁用")]
        public bool? EnableOrDisable { get; set; }
    }
}
