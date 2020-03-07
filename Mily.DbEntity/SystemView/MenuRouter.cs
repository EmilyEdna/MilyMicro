using Mily.Extension.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbEntity.SystemView
{
    /// <summary>
    /// 菜单路由
    /// </summary>
    [SugarTable("System_MenuRouter", "菜单路由表")]
    [DataSlice("Test")]
    public class MenuRouter:BaseEntity
    {
        /// <summary>
        /// 菜单表Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "菜单表Id")]
        public Guid? MenuItemId { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "功能名称")]
        public string FuncName { get; set; }
        /// <summary>
        /// 菜单路径
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "菜单路径")]
        public string MenuPath { get; set; }
        /// <summary>
        /// 功能路由
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "功能路由")]
        public string FuncRouterPath { get; set; }
    }
}
