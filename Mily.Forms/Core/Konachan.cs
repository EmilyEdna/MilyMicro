using Mily.Forms.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.HttpFactory;
using XExten.XPlus;

namespace Mily.Forms.Core
{
    public class Konachan
    {
        private const string BaseURL = "https://konachan.com/";
        private const string Tag = "tag.xml";
        private const string Pic = "post.xml?page=1&limit=8";
        public static Root GetPic()
        {
            var XmlData = HttpMultiClient.HttpMulti.AddNode(BaseURL + Pic, UseCache: true).Build().CacheTime(10).RunString();
            return XPlusEx.XmlDeserialize<Root>(XmlData.FirstOrDefault());
        }
        public void GetTag()
        {

        }
    }
}
