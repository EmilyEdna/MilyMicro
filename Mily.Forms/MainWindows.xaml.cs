using Mily.Forms.DataModel;
using System.Windows.Controls;
using System;

namespace Mily.Forms
{
    /// <summary>
    /// Test.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindows
    {
        public MainWindows()
        {
            InitializeComponent();
            Gloads = Gload;
        }

        private void Next_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var data = ModelView.GetChildObjects<CheckBox>(CustomerTemplate, "");
            });
        }

        private void Pre_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var data = ModelView.GetChildObjects<CheckBox>(CustomerTemplate, "");
            });
        }
    }

}
