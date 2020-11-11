using Mily.Forms.UI.PageUI;
using Mily.Forms.Utils;
using Mily.Forms.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Mily.Forms.ViewModel
{
    public class MainView : BaseView
    {
        public MainView() 
        {
            CurrentPage = Help.WelComePage;
        }

        #region Property
        private Page _CurrentPage;
        public Page CurrentPage {
            get { return _CurrentPage; }
            set {
                _CurrentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
        #endregion

        #region Commands
        public Commands<string> SelectedPage 
        {
            get {
                return new Commands<string>((str) => 
                {
                    if (str.Equals("Home"))
                        CurrentPage = Help.WelComePage;
                    else
                        CurrentPage = Help.KonachanPage;

                }, () => true);
            }
        }
        #endregion
    }
}
