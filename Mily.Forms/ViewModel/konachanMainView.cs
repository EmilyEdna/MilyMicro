using Mily.Forms.Core;
using Mily.Forms.DataModel;
using Mily.Forms.Utils;
using Mily.Forms.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using XExten.XCore;

namespace Mily.Forms.ViewModel
{
    public class konachanMainView : BaseView
    {
        public static Dictionary<string, konachanMainView> Ioc = new Dictionary<string, konachanMainView>();
        public konachanMainView()
        {
            RootData = Konachan.GetPic(1);
            CurrentPage = 1;
            if (!Ioc.ContainsKey(GetType().Name))
                Ioc.Add(GetType().Name, this);
        }

        public static readonly Dictionary<long, string> Path = new Dictionary<long, string>();

        #region Property
        private ImageRoot _RootDate;
        public ImageRoot RootData
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

        //private List<CustomerTag> _Json;
        //public List<CustomerTag> Json
        //{
        //    get
        //    {
        //        return _Json.OrderByDescending(t => t.AddTime).ToList();
        //    }
        //    set
        //    {
        //        _Json = value;
        //        OnPropertyChanged("Json");
        //    }
        //}
        //private CustomerTag _SelectItem;
        //public CustomerTag SelectItem
        //{
        //    get
        //    {
        //        return _SelectItem;
        //    }
        //    set
        //    {
        //        _SelectItem = value;
        //        OnPropertyChanged("SelectItem");
        //    }
        //}

        //private string _SelectValue;
        //public string SelectedValue
        //{
        //    get { return _SelectValue; }
        //    set
        //    {
        //        _SelectValue = value;
        //        OnPropertyChanged("SelectedValue");
        //    }
        //}
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
                    //if (!SelectedValue.IsNullOrEmpty() && SelectItem != null)
                    //    RootData = Konachan.GetPic(CurrentPage, SelectedValue);
                    //else
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
                        //if (!SelectedValue.IsNullOrEmpty() && SelectItem != null)
                        //    RootData = Konachan.GetPic(CurrentPage, SelectedValue);
                        //else
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
                    //if (!SelectedValue.IsNullOrEmpty() && SelectItem != null)
                    //    RootData = Konachan.GetPic(1, SelectedValue);
                    //else
                    RootData = Konachan.GetPic(1);

                }, () => true);
            }
        }
        #endregion
    }
}
