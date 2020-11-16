using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.DataModel.SqlModel
{
    [SugarTable("KonaTag")]
    public class KonaTag
    {
        public int Id { get; set; }
        [SugarColumn(ColumnDataType = "varchar(300)")]
        public string TagName { get; set; }
        public int Count { get; set; }
    }
}
