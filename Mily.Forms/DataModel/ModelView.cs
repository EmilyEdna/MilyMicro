using Mily.Forms.Core;
using Mily.Forms.DataModel.ViewModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using XExten.XCore;

namespace Mily.Forms.DataModel
{
    public class ModelView : BaseView
    {
        public static Dictionary<string, ModelView> Ioc = new Dictionary<string, ModelView>();
        public ModelView()
        {
            RootData = Konachan.GetPic(1);
            CurrentPage = 1;
            Json = Read(out _);
            if (!Ioc.ContainsKey(GetType().Name))
                Ioc.Add(GetType().Name, this);
        }

        public static readonly Dictionary<long, string> Path = new Dictionary<long, string>();

        #region Property
        private Root _RootDate;
        public Root RootData
        {
            get
            {
                return _RootDate;
            }
            set
            {
                _RootDate = value;
                OnPropertyChanged("RootData");
            }
        }

        private int _CurrentPage;
        public int CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                _CurrentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private List<JsonTag> _Json;
        public List<JsonTag> Json
        {
            get
            {
                return _Json.OrderByDescending(t => t.AddTime).ToList();
            }
            set
            {
                _Json = value;
                OnPropertyChanged("Json");
            }
        }
        private JsonTag _SelectItem;
        public JsonTag SelectItem
        {
            get
            {
                return _SelectItem;
            }
            set
            {
                _SelectItem = value;
                OnPropertyChanged("SelectItem");
            }
        }

        private string _SelectValue;
        public string SelectedValue
        {
            get { return _SelectValue; }
            set
            {
                _SelectValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }
        #endregion

        #region Command
        public Commands<object> NextPage
        {
            get
            {
                return new Commands<object>((obj) =>
                {
                    if (Path.Count != 0)
                        Path.Clear();
                    CurrentPage += 1;
                    if (!SelectedValue.IsNullOrEmpty() && SelectItem != null)
                        RootData = Konachan.GetPic(CurrentPage, SelectedValue);
                    else
                        RootData = Konachan.GetPic(CurrentPage);
                }, () => true);
            }
        }
        public Commands<object> PrePage
        {
            get
            {
                return new Commands<object>((obj) =>
                {
                    if (CurrentPage > 1)
                    {
                        if (Path.Count != 0)
                            Path.Clear();
                        CurrentPage -= 1;
                        if (!SelectedValue.IsNullOrEmpty() && SelectItem != null)
                            RootData = Konachan.GetPic(CurrentPage, SelectedValue);
                        else
                            RootData = Konachan.GetPic(CurrentPage);
                    }
                }, () => true);
            }
        }
        public Commands<Dictionary<long, string>> CheckPic
        {
            get
            {
                return new Commands<Dictionary<long, string>>((param) =>
                {
                    foreach (var item in param)
                    {
                        if (Path.ContainsKey(item.Key))
                            Path.Remove(item.Key);
                        else
                            Path.Add(item.Key, item.Value);
                    }
                }, () => true);
            }
        }
        public Commands<string> ReturnPage
        {
            get
            {
                return new Commands<string>((No) =>
                {
                    int.TryParse(No, out int Page);
                    CurrentPage = Page <= 0 ? 1 : Page;
                    RootData = Konachan.GetPic(CurrentPage);
                }, () => true);
            }
        }
        public Commands<object> Search
        {
            get
            {
                return new Commands<object>((obj) =>
                {
                    CurrentPage = 1;
                    if (!SelectedValue.IsNullOrEmpty() && SelectItem != null)
                        RootData = Konachan.GetPic(1, SelectedValue);
                    else
                        RootData = Konachan.GetPic(1);

                }, () => true);
            }
        }
        #endregion

        #region Common
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private List<JsonTag> Read(out string Path)
        {
            var BasePath = AppDomain.CurrentDomain.BaseDirectory + "config.cof";
            Path = BasePath;
            if (!File.Exists(BasePath))
                File.Create(BasePath).Dispose();
            using StreamReader reader = new StreamReader(BasePath);
            var res = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            return !res.IsNullOrEmpty() ? res.ToLzStringDec().ToModel<List<JsonTag>>() : new List<JsonTag>();
        }

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
        #endregion
    }
}
