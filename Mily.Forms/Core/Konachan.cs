using Mily.Forms.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.HttpFactory;
using XExten.XCore;
using XExten.XPlus;


namespace Mily.Forms.Core
{
    public class Konachan
    {
        private const string BaseURL = "https://konachan.com/";
        private const string Tag = "tag.xml?order=date&limit={0}";
        private const string Pic = "post.xml?page={0}&limit=8";
        public static Root GetPic(int Page, string Tag = "")
        {
            var Hosts = string.Format(Pic, Page);
            if (!Tag.IsNullOrEmpty())
                Hosts += $"&tags={Tag}";
            var XmlData = HttpMultiClient.HttpMulti.AddNode(BaseURL + Hosts, UseCache: true).Build().CacheTime().RunString();
            return XPlusEx.XmlDeserialize<Root>(XmlData.FirstOrDefault());
        }
        public static long GetTotalTag()
        {
            var XmlData = HttpMultiClient.HttpMulti.AddNode(BaseURL + string.Format(Tag, 1)).Build().CacheTime().RunString();
            return XPlusEx.XmlDeserialize<Tags>(XmlData.FirstOrDefault()).Post.FirstOrDefault().Id;
        }
        public void GetTag()
        {

        }
    }
}
