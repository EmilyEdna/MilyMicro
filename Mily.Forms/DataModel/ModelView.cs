using Mily.Forms.Core;
using Mily.Forms.DataModel.ViewModel;
using System;
using System.Collections.Generic;
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
        public ModelView()
        {
            RootData = Konachan.GetPic(1);
            CurrentPage = 1;
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
        #endregion

        #region Command
        public Commands<object> NextPage
        {
            get
            {
                return new Commands<object>(GoNext, CanRun);
            }
        }
        public Commands<object> PrePage
        {
            get
            {
                return new Commands<object>(GoPre, CanRun);
            }
        }
        public Commands<Dictionary<long, string>> CheckPic
        {
            get
            {
                return new Commands<Dictionary<long, string>>(Check, CanRun);
            }
        }
        public Commands<string> ReturnPage
        {
            get
            {
                return new Commands<string>(PageGo, CanRun);
            }
        }
        private void PageGo(string No)
        {
            int.TryParse(No, out int Page);
            CurrentPage = Page == 0 ? 1 : Page;
            RootData = Konachan.GetPic(CurrentPage);
        }
        private void Check(Dictionary<long, string> param)
        {
            foreach (var item in param)
            {
                if (Path.ContainsKey(item.Key))
                    Path.Remove(item.Key);
                else
                    Path.Add(item.Key, item.Value);
            }
        }
        private void GoNext(object param)
        {
            if (Path.Count != 0)
                Path.Clear();
            CurrentPage += 1;
            RootData = Konachan.GetPic(CurrentPage);


        }
        private void GoPre(object param)
        {
            if (CurrentPage > 1)
            {
                if (Path.Count != 0)
                    Path.Clear();
                CurrentPage -= 1;
                RootData = Konachan.GetPic(CurrentPage);
            }
        }
        private bool CanRun() => true;
        #endregion

        #region Common


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
