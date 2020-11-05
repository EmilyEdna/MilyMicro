using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mily.Forms.DataModel.ViewModel
{
    public class BaseView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
