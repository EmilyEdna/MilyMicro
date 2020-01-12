using Mily.Extension.Attributes;
using SqlSugar;

namespace Mily.DbEntity.SystemView
{
    /// <summary>
    /// 消费记录日志表
    /// </summary>
    [SugarTable("System_RabbitMQLog", "消费记录日志表")]
    [DataSlice("Test")]
    public class RabbitMQLog : BaseEntity
    {
        /// <summary>
        /// 日志名称
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "日志名称", Length = 50)]
        public string LogName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "来源", Length = 50)]
        public string Source { get; set; }

        /// <summary>
        /// 事件源
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "事件源", Length = 100)]
        public string EventData { get; set; }
    }
}