using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.TreeViewTest.Directory.Data
{
    public class DirectoryItem
    {
        public string FullPath { get; set; }
        public string Name => Type == DirectoryItemType.Diver ? FullPath : DirectoryStucture.GetFileOrFolderName(FullPath);
        public DirectoryItemType Type { get; set; }
    }
}
