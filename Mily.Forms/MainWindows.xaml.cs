using Mily.Forms.DataModel;
using System.Windows.Controls;
using System;
using System.Threading.Tasks;
using Mily.Forms.Core;
using XExten.XPlus;
using System.IO;

namespace Mily.Forms
{
    /// <summary>
    /// Test.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindows
    {
        public MainWindows()
        {
            Task.Factory.StartNew(() => Konachan.LoadTagToLocal());
            InitializeComponent();
            Gloads = Gload;
        }
    }
}
