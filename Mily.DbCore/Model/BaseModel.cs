using Mily.Setting.ModelEnum;
using SqlSugar;
using System;

namespace Mily.DbCore.Model
{
    /// <summary>
    /// 所有实体的父类
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public Guid KeyId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否删除")]
        public bool? IsDelete { get; set; }
    }
}