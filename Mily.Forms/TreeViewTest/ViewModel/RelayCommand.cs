using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Mily.Forms.TreeViewTest.ViewModel
{
    public class RelayCommand : ICommand
    {
        private Action mAction;
        public event EventHandler CanExecuteChanged = (sender, e) => { };
        public RelayCommand(Action action)
        {
            mAction = action;
        }
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            mAction();
        }
    }
}
