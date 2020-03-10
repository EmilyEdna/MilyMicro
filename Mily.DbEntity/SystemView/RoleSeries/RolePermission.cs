using Mily.Extension.Attributes;
using Mily.Setting.ModelEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbEntity.SystemView.RoleSeries
{
    /// <summary>
    /// 权限许可表
    /// </summary>
    [SugarTable("System_RolePermission", "权限许可表")]
    [DataSlice("Test")]
    public class RolePermission : BaseEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "角色名称", Length = 50)]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "INT", ColumnDescription = "角色类型")]
        public RoleTypeEnum? RoleType { get; set; }
    }
}
