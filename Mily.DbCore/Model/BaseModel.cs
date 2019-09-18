﻿using Mily.Setting.ModelEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

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
        /// 创建人
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "创建人", Length = 50)]
        public string CreateUser { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "修改人", Length = 50)]
        public string UpdateUser { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "删除人", Length = 50)]
        public string DeleteUser { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建人Id")]
        public Guid? CreateUserId { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改人Id")]
        public Guid? UpdateUserId { get; set; }
        /// <summary>
        /// 删除人Id
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除人Id")]
        public Guid? DeleteUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "是否删除")]
        public bool? IsDelete { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "审核状态")]
        public AuditStatusEnum? AuditStatus { get; set; }
    }
}
