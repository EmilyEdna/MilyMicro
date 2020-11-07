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
        private const string Pic = "post.xml?page={0}&limit=8";
        public static Root GetPic(int Page)
        {
            var XmlData = HttpMultiClient.HttpMulti.AddNode(BaseURL + string.Format(Pic, Page), UseCache: true).Build().CacheTime().RunString();
            return XPlusEx.XmlDeserialize<Root>(XmlData.FirstOrDefault());
        }
        public void GetTag()
        {

        }
    }
}
