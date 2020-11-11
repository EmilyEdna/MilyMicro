using Mily.Forms.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Mily.Forms.UI.PageUI
{
    /// <summary>
    /// KonachanPage.xaml 的交互逻辑
    /// </summary>
    public partial class KonachanPage : Page
    {
        private readonly List<string> Data;
        private readonly Dictionary<string, ContextMenu> Ioc = new Dictionary<string, ContextMenu>();
        public KonachanPage()
        {
            InitializeComponent();
            MouseRightButtonDown += KonachanPage_MouseRightButtonDown;
            Data = new List<string>{
                "下载",
                "打开目录",
                "标签查询",
                "自定义标签"
            };
        }
        private void KonachanPage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            KonaPage.ContextMenu = GetRightMenu();
        }

        private ContextMenu GetRightMenu()
        {
            if (Ioc.ContainsKey("Right")) return Ioc["Right"];
            else
            {
                ContextMenu Menu = new ContextMenu();
                CreateMenu(Menu, Data);
                Ioc.Add("Right", Menu);
                return Menu;
            }
        }

        private void CreateMenu(ContextMenu Menu, List<string> Title)
        {
            Title.ForEach(t =>
            {
                MenuItem Item = new MenuItem();
                Item.Header = t;
                Item.Click += Item_Click;
                Menu.Items.Add(Item);
            });
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            var Item = (sender as MenuItem);
            if (Item.Header.ToString().Equals("打开目录"))
            {
                string Path = AppDomain.CurrentDomain.BaseDirectory + "SaveImg";
                if (Directory.Exists(Path) == false)
                    Directory.CreateDirectory(Path);
                Process.Start("explorer.exe", Path);
            }
            else if (Item.Header.ToString().Equals("下载"))
            {
                MessageBox.Show("下载时候软件会进进入假死状态！请不要关闭！", "通知", MessageBoxButton.OK);
                if (KonachanMainView.Path.Count == 0)
                    MessageBox.Show("你还未选择需要下载的图片！", "通知", MessageBoxButton.OK);
                else
                {
                    foreach (var item in KonachanMainView.Path)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            string NewName = Guid.NewGuid() + Path.GetExtension(item.Value);
                            string Paths = AppDomain.CurrentDomain.BaseDirectory + "SaveImg\\";
                            if (Directory.Exists(Paths) == false)
                                Directory.CreateDirectory(Paths);
                            new WebClient().DownloadFile(item.Value, Paths + NewName);
                        });
                    }
                    MessageBox.Show("下载完成！", "通知", MessageBoxButton.OK);
                }
            }
            else if (Item.Header.ToString().Equals("标签查询"))
            {
                TagSearch win = new TagSearch
                {
                    Width = 500,
                    Height = 265
                };
                win.Left = SystemParameters.PrimaryScreenWidth / 3;
                win.Top = SystemParameters.PrimaryScreenHeight / 3;
                win.ShowDialog();
            }
            else
            {
                CustomerTag win = new CustomerTag
                {
                    Width = 400,
                    Height = 265
                };
                win.Left = SystemParameters.PrimaryScreenWidth / 3;
                win.Top = SystemParameters.PrimaryScreenHeight / 3;
                win.ShowDialog();
            }
        }
    }
}
