using Mily.Forms.Core;
using Mily.Forms.DataModel.Konochan;
using Mily.Forms.Utils;
using Mily.Forms.ViewModel.Base;
using System.Collections.Generic;
using XExten.XCore;

namespace Mily.Forms.ViewModel
{
    public class KonachanMainView : BaseView
    {
        public static readonly Dictionary<string, KonachanMainView> Ioc = new Dictionary<string, KonachanMainView>();
        public static readonly Dictionary<long, string> Path = new Dictionary<long, string>();
        public string KeyWord { get; set; }
        public KonachanMainView()
        {
            RootData = Konachan.GetPic(1);
            CurrentPage = 1;
            if (!Ioc.ContainsKey(GetType().Name))
                Ioc.Add(GetType().Name, this);
        }

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
                    if(!KeyWord.IsNullOrEmpty())
                         RootData = Konachan.GetPic(CurrentPage,KeyWord);
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
                        if (!KeyWord.IsNullOrEmpty())
                            RootData = Konachan.GetPic(CurrentPage, KeyWord);
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
                    if (!KeyWord.IsNullOrEmpty())
                        RootData = Konachan.GetPic(CurrentPage, KeyWord);
                    else
                        RootData = Konachan.GetPic(CurrentPage);
                }, () => true);
            }
        }
        #endregion

        public void Search(string key) 
        {
            KeyWord = key;
            CurrentPage = 1;
            RootData = Konachan.GetPic(1,key);
        }
    }
}
