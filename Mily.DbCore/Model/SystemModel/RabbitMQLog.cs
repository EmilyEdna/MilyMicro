using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbCore.Model.SystemModel
{
    [SugarTable("System_RabbitMQLog", "消费记录日志表")]
    public class RabbitMQLog:BaseModel
    {
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "日志名称", Length = 50)]
        public string LogName { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "来源", Length = 50)]
        public string Source { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "事件源", Length = 100)]
        public string EventData { get; set; }
    }
}
