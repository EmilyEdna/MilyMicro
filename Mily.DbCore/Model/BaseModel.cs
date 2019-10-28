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
        /// 逻辑删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "逻辑删除")]
        public bool? Deleted { get; set; }
    }
}