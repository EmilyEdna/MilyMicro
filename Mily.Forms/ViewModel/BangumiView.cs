using Mily.Forms.Core;
using Mily.Forms.DataModel.Imomoe;
using Mily.Forms.UI.PlayUI;
using Mily.Forms.Utils;
using Mily.Forms.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.ViewModel
{
    public class BangumiView : BaseView
    {
        public string Kw { get; set; }
        public static readonly Dictionary<string, BangumiView> Ioc = new Dictionary<string, BangumiView>();
        public BangumiView()
        {
            CurrentPage = 1;
            if (!Ioc.ContainsKey(GetType().Name))
                Ioc.Add(GetType().Name, this);
        }

        #region Property
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

        private string _Info;
        public string Info
        {
            get
            {
                return _Info;
            }
            set
            {
                _Info = value;
                OnPropertyChanged("Info");
            }
        }

        private SearchRoot _Sukura;
        public SearchRoot Sukura
        {
            get
            {
                return _Sukura;
            }
            set
            {
                _Sukura = value;
                OnPropertyChanged("Sukura");
            }
        }

        private DetailRoot _SukuraDetail;
        public DetailRoot SukuraDetail
        {
            get
            {
                return _SukuraDetail;
            }
            set
            {
                _SukuraDetail = value;
                OnPropertyChanged("SukuraDetail");
            }
        }
        #endregion

        #region Commands
        public Commands<string> ShowPage
        {
            get
            {
                return new Commands<string>((str) =>
                {
                    Info = "介绍：";
                    SukuraDetail = Imomoe.GetBangumiPage(str);
                }, () => true);
            }
        }
        public Commands<string> WacthPage
        {
            get
            {
                return new Commands<string>((str) =>
                {
                    BangumiFull full = new BangumiFull()
                    {
                        MediaURL = new Uri(Imomoe.GetVedio(str))
                    };
                    full.Show();
                }, () => true);
            }
        }
        public Commands<object> NextPage
        {
            get
            {
                return new Commands<object>((obj) =>
                {
                    if (CurrentPage < Sukura.TotalPage)
                    {
                        CurrentPage += 1;
                        Sukura = Imomoe.GetBangumi(Kw, CurrentPage);
                    }
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
                        CurrentPage -= 1;
                        Sukura = Imomoe.GetBangumi(Kw, CurrentPage);
                    }
                }, () => true);
            }
        }
        public Commands<string> ReturnPage
        {
            get
            {
                return new Commands<string>((str) =>
                {
                    int.TryParse(str, out int Page);
                    CurrentPage = Page == 0 ? 1 : Page;
                    Sukura = Imomoe.GetBangumi(Kw, CurrentPage);
                }, () => true);
            }
        }
        #endregion

        /// <summary>
        /// 检索动漫列表
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="page"></param>
        public void Search(string KeyWord, int page = 1)
        {
            CurrentPage = 1;
            Kw = KeyWord;
            Sukura = Imomoe.GetBangumi(KeyWord, page);
        }
    }
}
