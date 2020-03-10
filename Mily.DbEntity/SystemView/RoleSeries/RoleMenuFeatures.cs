using Mily.Extension.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbEntity.SystemView.RoleSeries
{
    /// <summary>
    /// 权限菜单功能表
    /// </summary>
    [SugarTable("System_RoleMenuFeatures", "权限菜单功能表")]
    [DataSlice("Test")]
    public class RoleMenuFeatures:BaseEntity
    {
        /// <summary>
        /// 权限许可ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "权限许可ID")]
        public Guid? RolePermissionId { get; set; }

        /// <summary>
        /// 菜单表ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "菜单表ID")]
        public Guid? MenuItemId { get; set; }

        /// <summary>
        /// 菜单功能表ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "菜单功能表ID")]
        public Guid? MenuFeatId { get; set; }
    }
}
