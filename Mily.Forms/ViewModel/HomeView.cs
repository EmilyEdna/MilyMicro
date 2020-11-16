using Mily.Forms.DataModel.Konochan;
using Mily.Forms.Utils;
using Mily.Forms.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.XCore;

namespace Mily.Forms.ViewModel
{
    public class HomeView : BaseView
    {
        public static Dictionary<string, HomeView> Ioc = new Dictionary<string, HomeView>();
        public HomeView()
        {
            DropData = Help.Boxs();
            Json = Read();
            if (!Ioc.ContainsKey(GetType().Name))
                Ioc.Add(GetType().Name, this);
        }

        #region Property
        private string _CurrentDropData;
        public string CurrentDropData
        {
            get { return _CurrentDropData; }
            set
            {
                _CurrentDropData = value;
                OnPropertyChanged("CurrentDropData");
            }
        }

        private List<string> _DropData;
        public List<string> DropData
        {
            get { return _DropData; }
            set
            {
                _DropData = value;
                OnPropertyChanged("DropData");
            }
        }

        private List<CustomerTag> _Json;
        public List<CustomerTag> Json
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

        private CustomerTag _SelectItem;
        public CustomerTag SelectItem
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

        private string _TextValue;
        public string TextValue
        {
            get { return _TextValue; }
            set
            {
                _TextValue = value;
                OnPropertyChanged("TextValue");
            }
        }
        #endregion

        #region Commands
        public Commands<object> Search
        {
            get
            {
                return new Commands<object>((obj) =>
                {
                    if (CurrentDropData.IsNullOrEmpty()) return;
                    if (CurrentDropData.Equals("图片"))
                    {
                        KonachanMainView.Ioc.Values.FirstOrDefault().Search(SelectedValue);
                        MainView.Ioc.Values.FirstOrDefault().CurrentPage = Help.KonachanPage;
                    }
                    else if (CurrentDropData.Equals("动漫(名称)"))
                    {
                        BangumiView.Ioc.Values.FirstOrDefault().SearchForName(TextValue);
                        MainView.Ioc.Values.FirstOrDefault().CurrentPage = Help.BangumiPage;
                    }
                    else {
                        BangumiView.Ioc.Values.FirstOrDefault().SearchForTime(TextValue);
                        MainView.Ioc.Values.FirstOrDefault().CurrentPage = Help.BangumiPage;
                    }
                }, () => true);
            }
        }
        #endregion

        #region 读
        private List<CustomerTag> Read()
        {
            Help.FileCreater(Help.Config_cof);
            var res = Help.Read(Help.Config_cof);
            return !res.IsNullOrEmpty() ? res.ToLzStringDec().ToModel<List<CustomerTag>>() : new List<CustomerTag>();
        }
        #endregion
    }
}
