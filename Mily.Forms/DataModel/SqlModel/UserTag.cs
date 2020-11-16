using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.DataModel.SqlModel
{
    [SugarTable("UserTag")]
    public class UserTag
    {
        public string Key { get; set; }
        public string Value { get; set; }
        [SugarColumn(ColumnDataType = "DateTime")]
        public DateTime AddTime { get; set; }
    }
}
