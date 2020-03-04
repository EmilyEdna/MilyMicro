using Mily.Extension.Attributes;
using Mily.Setting.ModelEnum;
using SqlSugar;
using System;

namespace Mily.DbEntity.SystemView
{
    /// <summary>
    ///操作日志表
    /// </summary>
    [SugarTable("System_HandleLog", "操作日志表")]
    [DataSlice("Test")]
    public class SystemhandleLog : BaseEntity
    {
        /// <summary>
        /// 操作人
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "操作人", ColumnDataType = "NVARCHAR", Length = 50)]
        public string Hnadler { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "操作类型", ColumnDataType = "INT")]
        public HandleEnum HandleType { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "操作时间", ColumnDataType = "DATETIME", Length = 50)]
        public DateTime? HandleTime { get; set; }

        /// <summary>
        /// 操作对象
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "操作对象", ColumnDataType = "NVARCHAR", Length = 50)]
        public string HandleObject { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "操作名称", ColumnDataType = "NVARCHAR", Length = 50)]
        public string HandleName { get; set; }

        /// <summary>
        /// 操作明细
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "操作明细", ColumnDataType = "NVARCHAR", Length = 100)]
        public string HandleObvious { get; set; }
    }
}