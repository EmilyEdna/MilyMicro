using Mily.Extension.Attributes;
using SqlSugar;

namespace Mily.DbEntity.SystemView
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
        /// 操作角色
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "操作角色", Length = 50)]
        public string HandlerRole { get; set; }
    }
}