using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbCore.Model.SystemModel
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    [SugarTable("System_Administrator","系统用户表")]
    public class Administrator:BaseModel
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "管理员",Length =50)]
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
        [SugarColumn(IsNullable = false, ColumnDescription = "权限许可ID")]
        public Guid RolePermissionId { get; set; }
    }
}
