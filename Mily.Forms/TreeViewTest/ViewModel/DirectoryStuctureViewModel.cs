using Mily.Forms.TreeViewTest.Directory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Mily.Forms.TreeViewTest.ViewModel
{
    public class DirectoryStuctureViewModel:BaseViewModel
    {
        public DirectoryStuctureViewModel()
        {
            var Children = DirectoryStucture.GetLogicalDrives();
            Items = new ObservableCollection<DirectoryItemViewModel>(DirectoryStucture.GetLogicalDrives().Select(t=> new DirectoryItemViewModel(t.FullPath,t.Type)));
        }

        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }
    }
}
