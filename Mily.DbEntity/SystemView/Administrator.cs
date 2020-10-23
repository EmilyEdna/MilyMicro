using Mily.Extension.Attributes;
using SqlSugar;
using System;

namespace Mily.DbEntity.SystemView
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    [SugarTable("Sys_Admin", "系统用户表")]
    [DataSlice("Test")]
    public class Administrator : BaseEntity
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "管理员", Length = 50)]
        public string AdminName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "账号", Length = 50)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "密码", Length = 50)]
        public string PassWord { get; set; }

        /// <summary>
        /// 权限许可ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "权限许可ID")]
        public int? RolePermissionId { get; set; }
    }
}