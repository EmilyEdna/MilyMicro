using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.DataModel.Imomoe
{
    public class Elements
    {
        /// <summary>
        /// 封面
        /// </summary>
        public string Conver { get; set; }
        /// <summary>
        /// 详情页
        /// </summary>
        public string DetailPage { get; set; }
        /// <summary>
        /// 番剧名称
        /// </summary>
        public string BangumiName { get; set; }
    }
    public class SearchRoot
    {
        public int TotalPage => Convert.ToInt32(Math.Ceiling(Total / 20.0));
        public int Total { get; set; }
        public List<Elements> Post { get; set; }
    }
}
