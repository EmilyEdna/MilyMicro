using Mily.Forms.Core;
using Mily.Forms.DataModel.Imomoe;
using Mily.Forms.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.ViewModel
{
    public class BangumiView : BaseView
    {
        public static readonly Dictionary<string, BangumiView> Ioc = new Dictionary<string, BangumiView>();
        public BangumiView()
        {
            if (!Ioc.ContainsKey(GetType().Name))
                Ioc.Add(GetType().Name, this);
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

        /// <summary>
        /// 检索动漫列表
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="page"></param>
        public void Search(string KeyWord,int page=1)
        {
            Sukura = Imomoe.GetBangumi(KeyWord, page);
        }
    }
}
