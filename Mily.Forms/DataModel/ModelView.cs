using Mily.Forms.Core;
using Mily.Forms.DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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

        private readonly Dictionary<long, string> Path = new Dictionary<long, string>();

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
        public Commands<string> NextPage
        {
            get
            {
                return new Commands<string>(GoNext, CanRun);
            }
        }

        public Commands<string> PrePage
        {
            get
            {
                return new Commands<string>(GoPre, CanRun);
            }
        }
        public Commands<Dictionary<long, string>> CheckPic
        {
            get
            {
                return new Commands<Dictionary<long, string>>(Check, CanRun);
            }
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
        private void GoNext(string param)
        {
            CurrentPage += 1;
            RootData = Konachan.GetPic(CurrentPage);
        }
        private void GoPre(string param)
        {
            if (CurrentPage > 1)
            {
                CurrentPage -= 1;
                RootData = Konachan.GetPic(CurrentPage);
            }
        }
        private bool CanRun() => true;
        #endregion

    }
}
