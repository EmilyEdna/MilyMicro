using Mily.Forms.TreeViewTest.ViewModel;
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
            DataContext = new DirectoryStuctureViewModel();
        }
    }
}
