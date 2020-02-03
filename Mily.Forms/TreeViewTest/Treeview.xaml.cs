using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Mily.Forms.TreeViewTest
{
    /// <summary>
    /// Treeview.xaml 的交互逻辑
    /// </summary>
    public partial class Treeview : Window
    {
        public Treeview()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //获取本地磁盘
            foreach (var Drivers in Directory.GetLogicalDrives())
            {
                var Item = new TreeViewItem()
                {
                    //设置头和路径
                    Header = Drivers,
                    Tag = Drivers
                };
                //添加一个子类
                Item.Items.Add(null);
                //添加一个展开的事件
                Item.Expanded += View_Expanded;
                //子类添加到主treeview
                this.views.Items.Add(Item);
            }
        }

        private void View_Expanded(object sender, RoutedEventArgs e)
        {
            var Item = (TreeViewItem)sender;
            if (Item.Items[0] != null && Item.Items.Count != 1)
                return;
            Item.Items.Clear();
            var fullpath = (string)Item.Tag;
            //Dir
            var Directories = new List<string>();
            var dirs = Directory.GetDirectories(fullpath);
            if (dirs.Length > 0)
                Directories.AddRange(dirs);
            Directories.ForEach(t =>
            {
                var SubItem = new TreeViewItem()
                {
                    Header = GetFileOrFolderName(t),
                    Tag = t
                };
                SubItem.Items.Add(null);
                SubItem.Expanded += View_Expanded;
                Item.Items.Add(SubItem);
            });
            //File
            var File = new List<string>();
            var Files = Directory.GetFiles(fullpath);
            if (Files.Length > 0)
                File.AddRange(Files);
            File.ForEach(t =>
            {
                var SubItem = new TreeViewItem()
                {
                    Header = GetFileOrFolderName(t),
                    Tag = t
                };
                Item.Items.Add(SubItem);
            });
        }

        public static string GetFileOrFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            var normal = path.Replace("/", "\\");
            var last = normal.LastIndexOf("\\");
            return path.Substring(last + 1);
        }
    }
}
