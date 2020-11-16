using System;
using System.Collections.Generic;
using System.Text;
using XExten.HttpFactory;
using HtmlAgilityPack;
using System.Linq;
using Mily.Forms.DataModel.Imomoe;
using System.Text.RegularExpressions;
using XExten.XCore;
using XExten.XPlus;
using System.Windows;

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
        public static SearchRoot GetBangumi(int kw, int page = 0)
        {
            //每页15条数据
            var host = (page == 0 || page == 1) ? $"/{kw}" : $"/{kw}/{page}.html";
            var data = HttpMultiClient.HttpMulti.AddNode(BaseURL + host).Build().RunString().FirstOrDefault();
            return LoadSearchPage(data, 15);
        }

        public static DetailRoot GetBangumiPage(string Route)
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
        private static SearchRoot LoadSearchPage(string html, double pageNo = 20)
        {
            return XPlusEx.XTry(() =>
             {
                 SearchRoot roots = new SearchRoot
                 {
                     Post = new List<Elements>()
                 };
                 HtmlDocument document = new HtmlDocument();
                 document.LoadHtml(html);
                 var data = document.DocumentNode.SelectNodes("//div[@class='lpic']//li");
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
                     roots.TotalPage = Convert.ToInt32(Math.Ceiling(Total / pageNo));
                 }
                 else
                 {
                     roots.Total = 1;
                     roots.TotalPage = 1;
                 }
                 return roots;
             }, ex =>
             {
                 MessageBox.Show("未找到资源请检查检索名称是否正确！", "通知", MessageBoxButton.OK);
                 return null;
             });
        }
        private static DetailRoot LoadPlayPage(string html)
        {
            return XPlusEx.XTry(() =>
             {
                 DetailRoot roots = new DetailRoot
                 {
                     Post = new List<DetailPage>()
                 };
                 HtmlDocument document = new HtmlDocument();
                 document.LoadHtml(html);
                 var Nodes = document.DocumentNode;
                 roots.Conver = Nodes.SelectSingleNode("//div[@class='thumb l']//img").GetAttributeValue("src", "");
                 roots.Description = Nodes.SelectSingleNode("//div[@class='info']").InnerText.Replace("\r\n", "");
                 var data = Nodes.SelectNodes("//div[@class='movurl']//li");
                 if (data.Count != 0)
                 {
                     foreach (var item in data)
                     {
                         roots.Post.Add(new DetailPage
                         {
                             PlayPage = item.Descendants("a").FirstOrDefault().GetAttributeValue("href", ""),
                             Collection = item.Descendants("a").FirstOrDefault().InnerText.Replace("'", "")
                         }); ;
                     }
                 }
                 return roots;
             }, ex =>
             {
                 MessageBox.Show("未找到资源！", "通知", MessageBoxButton.OK);
                 return null;
             });
        }
        private static string LoadBangumi(string html)
        {
            return XPlusEx.XTry(() =>
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var node = document.DocumentNode.SelectSingleNode("//div[@class='play']//div[@data-vid]");
                var URL = node.GetAttributeValue("data-vid", "$").Replace("$mp4", "");
                if (URL.Contains(".mp4"))
                    return URL;
                var res = HttpMultiClient.HttpMulti.AddNode(URL).Build().RunString().FirstOrDefault();
                if (!res.IsNullOrEmpty())
                    return Regex.Match(res, "http(.*)").Value;
                return "";
            }, ex => null);
        }
        #endregion
    }
}
