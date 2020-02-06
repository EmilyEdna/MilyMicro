using Mily.Forms.TreeViewTest.Directory;
using Mily.Forms.TreeViewTest.Directory.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows.Input;

namespace Mily.Forms.TreeViewTest.ViewModel
{
    public class DirectoryItemViewModel : BaseViewModel
    {
        public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
        {
            FullPath = fullPath;
            Type = type;
            ExpandCommand = new RelayCommand(Expand);
            ClearChildren();
        }

        public string FullPath { get; set; }
        public DirectoryItemType Type { get; set; }
        public string Name => Type == DirectoryItemType.Diver ? FullPath : DirectoryStucture.GetFileOrFolderName(FullPath);
        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }
        public bool CanExpand => Type != DirectoryItemType.File;
        public bool IsExpanded
        {

            get
            {
                return Children?.Count(t => t != null) > 0;
            }
            set
            {
                if (value == true)
                    Expand();
                else
                    ClearChildren();
            }
        }
        public ICommand ExpandCommand { get; set; }
        private void ClearChildren()
        {
            Children = new ObservableCollection<DirectoryItemViewModel>();
            if (Type != DirectoryItemType.File)
                Children.Add(null);
        }
        private void Expand()
        {
            if (Type == DirectoryItemType.File)
                return;
            var Children = DirectoryStucture.GetDirectoryContents(this.FullPath);
            this.Children = new ObservableCollection<DirectoryItemViewModel>(DirectoryStucture.GetDirectoryContents(FullPath).Select(t => new DirectoryItemViewModel(t.FullPath, t.Type)));
        }
    }
}
