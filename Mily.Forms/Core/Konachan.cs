using Mily.Forms.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Windows;
using XExten.HttpFactory;
using XExten.XCore;
using XExten.XPlus;


namespace Mily.Forms.Core
{
    public class Konachan
    {
        private const string BaseURL = "https://konachan.com/";
        private const string Tag = "tag.xml?order=date&limit=0";
        private const string Pic = "post.xml?page={0}&limit=8";
        public static Root GetPic(int Page, string Tag = "")
        {
            try
            {
                var Hosts = string.Format(Pic, Page);
                if (!Tag.IsNullOrEmpty())
                    Hosts += $"&tags={Tag}";
                var XmlData = HttpMultiClient.HttpMulti.AddNode(BaseURL + Hosts, UseCache: true).Build().CacheTime().RunString();
                return XPlusEx.XmlDeserialize<Root>(XmlData.FirstOrDefault());
            }
            catch (Exception)
            {
                MessageBox.Show("多线程初始化中！请稍等！", "通知", MessageBoxButton.OK);
                return new Root();
            }

        }
        public static Tags GetTag()
        {
            try
            {
                var XmlData = HttpMultiClient.HttpMulti.AddNode(BaseURL + Tag).Build().RunString();
                return XPlusEx.XmlDeserialize<Tags>(XmlData.FirstOrDefault());
            }
            catch (Exception)
            {
                MessageBox.Show("多线程初始化中！请稍等！", "通知", MessageBoxButton.OK);
                return GetTag();
            }

        }
        private static List<TagElements> Cache { get; set; }
        public static void LoadTagToLocal()
        {
            var BasePath = AppDomain.CurrentDomain.BaseDirectory + "tags.xml";
            if (!File.Exists(BasePath))
            {
                File.Create(BasePath).Dispose();
                var data = XPlusEx.XmlSerializer(GetTag());
                Write(BasePath, data);
            }
            else
            {
                var Config = AppDomain.CurrentDomain.BaseDirectory + "config.json";
                if (!File.Exists(Config))
                    File.Create(Config).Dispose();
                var search = Read(Config)?.ToModel<Dictionary<string, string>>();
                if (search == null)
                {
                    List<int> Days = new List<int>
                    {
                        1,5,10,15,20,25,30
                     };
                    if (Days.Contains(DateTime.Now.Day))
                    {
                        var data = XPlusEx.XmlSerializer(GetTag());
                        Write(BasePath, data);
                        Write(Config, (new { Key = DateTime.Now.ToFmtDate(4, "yyyy-MM-dd") }).ToJson());
                    }
                }
                else
                {
                    if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) != DateTime.Parse(search.Values.FirstOrDefault()))
                    {
                        var data = XPlusEx.XmlSerializer(GetTag());
                        Write(BasePath, data);
                        Write(Config, (new { Key = DateTime.Now.ToFmtDate(4, "yyyy-MM-dd") }).ToJson());
                    }
                }
            }
        }
        public static IEnumerable<TagElements> LoadLocalTag(int PageNo, out int Total)
        {
            if (Cache != null)
            {
                Total = Cache.Count();
                return Cache.Skip((PageNo - 1) * 20).Take(20);
            }
            var BasePath = AppDomain.CurrentDomain.BaseDirectory + "tags.xml";
            var res = Read(BasePath);
            Cache = XPlusEx.XmlDeserialize<Tags>(res).Post;
            Total = Cache.Count();
            return XPlusEx.XmlDeserialize<Tags>(res).Post.Skip((PageNo - 1) * 20).Take(20);
        }
        public static string Read(string Path)
        {
            using StreamReader reader = new StreamReader(Path);
            var res = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            return res;
        }
        public static void Write(string Path, string data, Action action = null)
        {
            using StreamWriter writer = new StreamWriter(Path, false);
            XPlusEx.XTry(() =>
            {
                action?.Invoke();
                writer.Write(data);
            }, ex => throw ex, () =>
            {
                writer.Close();
                writer.Dispose();
            });
        }
    }
}
