﻿using Mily.Forms.Core.Sql;
using Mily.Forms.DataModel.Konochan;
using Mily.Forms.DataModel.SqlModel;
using Mily.Forms.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
        public static ImageRoot GetPic(int Page, string Tag = "")
        {
            try
            {
                var Hosts = string.Format(Pic, Page);
                if (!Tag.IsNullOrEmpty())
                    Hosts += $"&tags={Tag}";
                var XmlData = HttpMultiClient.HttpMulti.AddNode(BaseURL + Hosts, UseCache: true).Build().CacheTime().RunString();
                return XPlusEx.XmlDeserialize<ImageRoot>(XmlData.FirstOrDefault());
            }
            catch (Exception)
            {
                MessageBox.Show("多线程初始化中！请稍等！", "通知", MessageBoxButton.OK);
                return new ImageRoot();
            }

        }
        public static TagRoot GetTag(int page = 0)
        {
            try
            {
                var XmlData = HttpMultiClient.HttpMulti.AddNode(BaseURL + string.Format(Tag, page)).Build().RunString();
                return XPlusEx.XmlDeserialize<TagRoot>(XmlData.FirstOrDefault());
            }
            catch (Exception)
            {
                MessageBox.Show("多线程初始化中！请稍等！", "通知", MessageBoxButton.OK);
                return GetTag();
            }

        }
        private static List<TagElements> Cache { get; set; }

        public static void LoadTagForDb()
        {
            var data = DbContext.Db().Queryable<KonaTag>().OrderBy(t => t.Id, OrderByType.Desc).First();
            if (data == null)
            {
                var res = GetTag().Post.ToAutoMapper<TagElements, KonaTag>();
                DbContext.Db().Insertable(res).ExecuteCommand();
            }
            else
            {
                var res = GetTag(50).Post.ToAutoMapper<TagElements, KonaTag>().Where(t => t.Id > data.Id).ToList();
                if (res.Count != 0)
                    DbContext.Db().Insertable(res).ExecuteCommand();
            }

        }

        public static List<TagElements> LoadTag(int PageNo, out int Total)
        {
            Total = DbContext.Db().Queryable<KonaTag>().Select(t => t.Id).Count();
            return DbContext.Db().Queryable<KonaTag>().OrderBy(t => t.Id, OrderByType.Desc).ToPageList(PageNo, 20).ToAutoMapper<KonaTag, TagElements>();
        }



        [Obsolete]
        public static void LoadTagToLocal(bool AllawysExcute = false)
        {
            if (AllawysExcute)
            {
                Help.FileDeleteCreater(Help.Tags_xml, () =>
                {
                    var data = XPlusEx.XmlSerializer(GetTag());
                    Help.Write(Help.Tags_xml, data);
                });
            }
            else
            {
                var Flag = Help.FileCreater(Help.Tags_xml, () =>
                {
                    var data = XPlusEx.XmlSerializer(GetTag());
                    Help.Write(Help.Tags_xml, data);
                });
                if (!Flag)
                {
                    Help.FileCreater(Help.Config_json);
                    var Search = Help.Read(Help.Config_json)?.ToModel<Dictionary<string, string>>();
                    if (Search == null)
                    {
                        List<int> Days = new List<int>
                    {
                        1,5,10,15,20,25,30
                     };
                        if (Days.Contains(DateTime.Now.Day))
                        {
                            var data = XPlusEx.XmlSerializer(GetTag());
                            Help.Write(Help.Tags_xml, data);
                            Help.Write(Help.Config_json, (new { Key = DateTime.Now.ToFmtDate(4, "yyyy-MM-dd") }).ToJson());
                        }
                    }
                    else
                    {
                        if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) != DateTime.Parse(Search.Values.FirstOrDefault()))
                        {
                            var data = XPlusEx.XmlSerializer(GetTag());
                            Help.Write(Help.Tags_xml, data);
                            Help.Write(Help.Config_json, (new { Key = DateTime.Now.ToFmtDate(4, "yyyy-MM-dd") }).ToJson());
                        }
                    }
                }
            }
        }
        [Obsolete]
        public static List<TagElements> LoadLocalTag(int PageNo, out int Total)
        {
            if (Cache != null)
            {
                Total = Cache.Count();
                return Cache.Skip((PageNo - 1) * 20).Take(20).ToList();
            }
            var res = Help.Read(Help.Tags_xml);
            if (res.IsNullOrEmpty())
            {
                LoadTagToLocal(true);
                res = Help.Read(Help.Tags_xml);
            }
            Cache = XPlusEx.XmlDeserialize<TagRoot>(res).Post;
            Total = Cache.Count();
            return XPlusEx.XmlDeserialize<TagRoot>(res).Post.Skip((PageNo - 1) * 20).Take(20).ToList();
        }
    }
}
