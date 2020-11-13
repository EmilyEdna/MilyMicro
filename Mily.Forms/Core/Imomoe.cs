using System;
using System.Collections.Generic;
using System.Text;
using XExten.HttpFactory;
using HtmlAgilityPack;
using System.Linq;
using Mily.Forms.DataModel.Imomoe;
using System.Text.RegularExpressions;
using XExten.XCore;

namespace Mily.Forms.Core
{
    public class Imomoe
    {
        private const string BaseURL = "http://www.yhdm.io";
        private const string Search = "/search/{0}/?page={1}";

        public static SearchRoot GetBangumi(string kw, int page = 1)
        {
            //每页20条数据
            var data = HttpMultiClient.HttpMulti.AddNode(BaseURL + string.Format(Search, kw, page)).Build().RunString().FirstOrDefault();
            return LoadSearchPage(data);
        }

        public static List<DetailRoot> GetBangumiPage(string Route)
        {
            var data = HttpMultiClient.HttpMulti.AddNode(BaseURL + Route).Build().RunString().FirstOrDefault();
            return LoadPlayPage(data);
        }

        public static string GetVedio(string PlayHtml)
        {
            var data = HttpMultiClient.HttpMulti.AddNode(BaseURL + PlayHtml).Build().RunString().FirstOrDefault();
            return LoadBangumi(data);
        }

        #region 爬虫
        private static SearchRoot LoadSearchPage(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var data = document.DocumentNode.SelectNodes("//div[@class='lpic']//li");
            SearchRoot roots = new SearchRoot
            {
                Post = new List<Elements>()
            };
            if (data.Count != 0)
            {
                foreach (var item in data)
                {
                    roots.Post.Add(new Elements
                    {
                        Conver = item.Descendants("img").FirstOrDefault().GetAttributeValue("src", ""),
                        DetailPage = item.Descendants("a").FirstOrDefault().GetAttributeValue("href", ""),
                        BangumiName = item.Descendants("img").FirstOrDefault().GetAttributeValue("alt", "")
                    });
                }
            }
            var page = document.DocumentNode.SelectSingleNode("//div[@class='pages']");
            if (page != null)
            {
                int.TryParse(Regex.Match(page.Descendants("a").FirstOrDefault().InnerText, "\\d+").Value, out int Total);
                roots.Total = Total;
            }
            return roots;
        }
        private static List<DetailRoot> LoadPlayPage(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var data = document.DocumentNode.SelectNodes("//div[@class='movurl']//li");
            List<DetailRoot> roots = new List<DetailRoot>();
            if (data.Count != 0)
            {
                foreach (var item in data)
                {
                    roots.Add(new DetailRoot
                    {
                        PlayPage = item.Descendants("a").FirstOrDefault().GetAttributeValue("href", ""),
                        Collection = item.Descendants("a").FirstOrDefault().InnerText
                    });
                }
            }
            return roots;
        }
        private static string LoadBangumi(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var node = document.DocumentNode.SelectSingleNode("//div[@class='play']//div[@data-vid]");
            var URL = node.GetAttributeValue("data-vid", "$").Replace("$mp4", "");
            var res = HttpMultiClient.HttpMulti.AddNode(URL).Build().RunString().FirstOrDefault();
            if (!res.IsNullOrEmpty())
              return  Regex.Match(res, "http(.*)").Value;
            return "";
        }
        #endregion
    }
}
