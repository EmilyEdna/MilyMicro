using Mily.Forms.DataModel;
using Mily.Forms.UI.PageUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using XExten.XPlus;

namespace Mily.Forms.Utils
{
    public class Help
    {
        public static string Config_cof = AppDomain.CurrentDomain.BaseDirectory + "config.cof";
        public static string Tags_xml = AppDomain.CurrentDomain.BaseDirectory + "tags.xml";
        public static string Config_json = AppDomain.CurrentDomain.BaseDirectory + "config.json";
        public static string SqlLite = AppDomain.CurrentDomain.BaseDirectory + "db.sqllite";

        #region 页面实例
        public static WelCome WelComePage = new WelCome();
        public static KonachanPage KonachanPage = new KonachanPage();
        public static SakuraBangumiPage BangumiPage = new SakuraBangumiPage();
        #endregion

        #region 读写公用
        public static bool FileCreater(string Path, Action action = null)
        {
            bool Flag = !File.Exists(Path);
            if (Flag)
            {
                File.Create(Path).Dispose();
                action?.Invoke();
            }
            return Flag;
        }
        public static void FileDeleteCreater(string Path, Action action = null)
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
                File.Create(Path).Dispose();
                action?.Invoke();
            }
            else {
                File.Create(Path).Dispose();
                action?.Invoke();
            }
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
        #endregion

        /// <summary>
        /// 获取子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, ""));//指定集合的元素添加到List队尾  
            }
            return childList;
        }

        public static List<string> Boxs()
        {
            return new List<string> { "图片","动漫(年份)","动漫(名称)" };
        }

    }
}
