using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mily.Forms.UI.PlayUI
{
    /// <summary>
    /// BangumiFull.xaml 的交互逻辑
    /// </summary>
    public partial class BangumiFull : Window
    {
        public  Uri MediaURL { get; set; }
        public BangumiFull()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bangumi.Source = MediaURL;
            Bangumi.Play();
        }
    }
}
