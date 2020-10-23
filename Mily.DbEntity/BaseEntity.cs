using SqlSugar;
using System;

namespace Mily.DbEntity
{
    /// <summary>
    /// 所有实体的父类
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, ColumnDescription = "主键", IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "逻辑删除")]
        public bool? Deleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "DATETIME", ColumnDescription = "创建时间")]
        public DateTime? Created { get; set; }
    }
}
