using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mily.Forms.TreeViewTest.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}

