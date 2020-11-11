using Mily.Forms.Core;
using Mily.Forms.DataModel;
using Mily.Forms.Utils;
using Mily.Forms.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Mily.Forms.ViewModel
{
    public class KonachanTagView : BaseView
    {
        public KonachanTagView()
        {
            CustomerTag = Konachan.LoadLocalTag(1, out int Total).ToList();
            Count = Total;
            TotalPage = Convert.ToInt64(Math.Ceiling(Count / 20.0));
            CurrentPage = 1;
        }

        #region Property
        private int _Count;
        public int Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged("Count");
            }
        }

        private long _TotalPage;
        public long TotalPage
        {
            get { return _TotalPage; }
            set
            {
                _TotalPage = value;
                OnPropertyChanged("TotalPage");
            }
        }

        private List<TagElements> _CustomerTag;
        public List<TagElements> CustomerTag
        {
            get { return _CustomerTag; }
            set
            {
                _CustomerTag = value;
                OnPropertyChanged("CustomerTag");
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
                    CurrentPage += 1;
                    CustomerTag = Konachan.LoadLocalTag(CurrentPage, out int Total).ToList();
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
                        CustomerTag = Konachan.LoadLocalTag(CurrentPage, out int Total).ToList();
                    }
                }, () => true);
            }
        }
        public Commands<string> ReturnPage
        {
            get
            {
                return new Commands<string>((obj) =>
                {
                    int.TryParse(obj, out int Page);
                    CurrentPage = Page <= 0 ? 1 : Page;
                    CustomerTag = Konachan.LoadLocalTag(CurrentPage, out int Total).ToList();
                }, () => true);
            }
        }
        public Commands<string> Copy
        {
            get
            {
                return new Commands<string>((obj) =>
                {
                    Clipboard.SetDataObject(obj);
                }, () => true);
            }
        }
        #endregion
    }
}
