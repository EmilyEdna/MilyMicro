using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            foreach (var item in Directory.GetLogicalDrives())
            {
                var view = new TreeViewItem();
                view.Header = item;
                this.views.Items.Add(view);
            } 
        }
    }
}
