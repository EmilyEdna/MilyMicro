using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Mily.Forms.DataModel.Konochan
{
    [Serializable]
    [XmlRoot("tags")]
    public class TagRoot
    {
        /// <summary>
        /// 标签
        /// </summary>
        [XmlElement("tag")]
        public List<TagElements> Post { get; set; }

    }
    [Serializable]
    public class TagElements
    {
        /// <summary>
        /// Id
        /// </summary>
        [XmlAttribute("id")]
        public long Id { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        [XmlAttribute("name")]
        public string TagName { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [XmlAttribute("count")]
        public int Count { get; set; }
    }
}
