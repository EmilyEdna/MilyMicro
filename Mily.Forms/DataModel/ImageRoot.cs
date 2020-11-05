using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using XExten.XPlus;

namespace Mily.Forms.DataModel
{
    [Serializable]
    [XmlRoot("posts")]
    public class Root
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public double PageNo => Math.Ceiling(Total / 8.0);
        /// <summary>
        /// 总数
        /// </summary>
        [XmlAttribute("count")]
        public int Total { get; set; }
        [XmlElement("post")]
        public List<Elements> Post { get; set; }
    }

    [Serializable]
    public class Elements
    {
        /// <summary>
        /// Id
        /// </summary>
        [XmlAttribute("id")]
        public long Id { get; set; }
        /// <summary>
        /// 预览地址
        /// </summary>
        [XmlAttribute("preview_url")]
        public string PreviewURL { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        [XmlAttribute("jpeg_height")]
        public int Height { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        [XmlAttribute("jpeg_width")]
        public int Width { get; set; }
        private string _FileURL;
        /// <summary>
        /// 地址
        /// </summary>
        [XmlAttribute("file_url")]
        public string FileURL
        {
            get
            {
                return HttpUtility.UrlDecode(_FileURL);
            }
            set
            {
                _FileURL = value;
            }
        }
        /// <summary>
        /// 大小 单位:MB
        /// </summary>
        public double FileSizeMB => double.Parse((FileSize * 1.0 / (1024 * 1024)).ToString("0.0"));
        /// <summary>
        /// 大小 单位：B
        /// </summary>
        [XmlAttribute("file_size")]
        public long FileSize { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        [XmlAttribute("rating")]
        public string Rating { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate => XPlusEx.XConvertStamptime(CreateSpan);
        [XmlAttribute("created_at")]
        public string CreateSpan { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [XmlAttribute("tags")]
        public string Tag { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [XmlAttribute("author")]
        public string Author { get; set; }


    }
}
