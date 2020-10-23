using Mily.Extension.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbEntity.SystemView.MenuSeries
{
    /// <summary>
    /// 系统菜单路由表
    /// </summary>
    [SugarTable("Sys_MenuItemsRouter", "系统菜单路由表")]
    [DataSlice("Test")]
    public class MenuItemsRouter: BaseEntity
    {
        /// <summary>
        /// 菜单表ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "菜单表ID")]
        public int? MenuItemId { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "路由名称", Length = 50)]
        public string Title { get; set; }

        /// <summary>
        /// 路劲地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "路劲地址", Length = 50)]
        public string PathRoad { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "路由地址", Length = 50)]
        public string PathRouter { get; set; }
    }
}
