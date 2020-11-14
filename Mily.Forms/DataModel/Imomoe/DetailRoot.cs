using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.DataModel.Imomoe
{
    public class DetailRoot
    {
        /// <summary>
        /// 封面
        /// </summary>
        public string Conver { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Description { get; set; }
        public List<DetailPage> Post { get; set; }
    }
    public class DetailPage
    {
        /// <summary>
        /// 播放页
        /// </summary>
        public string PlayPage { get; set; }
        /// <summary>
        /// 集数
        /// </summary>
        public string Collection { get; set; }
    }
}
