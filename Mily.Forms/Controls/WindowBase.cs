using Mily.Forms.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Mily.Forms.Controls
{
    public partial class WindowBase : Window
    {
        public Grid Gloads;
        private readonly Dictionary<string, ContextMenu> Ioc = new Dictionary<string, ContextMenu>();
        private readonly List<string> Data;
        public WindowBase()
        {
            Data = new List<string>{
                "下载",
                "打开目录",
                "标签查询",
                "自定义标签"
            };
            InitEvent();
        }

        #region 公用

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
                if (ModelView.Path.Count == 0)
                    MessageBox.Show("你还未选择需要下载的图片！", "通知", MessageBoxButton.OK);
                else
                {
                    foreach (var item in ModelView.Path)
                    {
                        string NewName = Guid.NewGuid() + Path.GetExtension(item.Value);
                        string Paths = AppDomain.CurrentDomain.BaseDirectory + "SaveImg\\";
                        if (Directory.Exists(Paths) == false)
                            Directory.CreateDirectory(Paths);
                        new WebClient().DownloadFile(item.Value, Paths + NewName);
                    }
                    MessageBox.Show("下载完成！", "通知", MessageBoxButton.OK);
                }
            }
            else if (Item.Header.ToString().Equals("标签查询")) {
                TagSearch win = new TagSearch
                {
                    Width = 400,
                    Height = 265
                };
                WindowStartupLocation = WindowStartupLocation.Manual;
                win.Left = SystemParameters.PrimaryScreenWidth / 3;
                win.Top = SystemParameters.PrimaryScreenHeight / 3;
                win.ShowDialog();
            }
            else
            {
                SourceWindow win = new SourceWindow
                {
                    Width = 400,
                    Height = 265
                };
                WindowStartupLocation = WindowStartupLocation.Manual;
                win.Left = SystemParameters.PrimaryScreenWidth / 3;
                win.Top = SystemParameters.PrimaryScreenHeight / 3;
                win.ShowDialog();
            }
        }
        #endregion

        #region 事件
        IntPtr Prt = IntPtr.Zero;
        private void InitEvent()
        {

            SourceInitialized += WindowBase_SourceInitialized;
            MouseLeftButtonDown += WindowBase_MouseLeftButtonDown;
            MouseRightButtonDown += WindowBase_MouseRightButtonDown;
        }

        private void WindowBase_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid || e.OriginalSource is Window || e.OriginalSource is Border)
            {
                Gloads.ContextMenu = GetRightMenu();
            }
        }

        private void WindowBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid || e.OriginalSource is Window || e.OriginalSource is Border)
            {
                SendMessage(Prt, 0x00A1, (IntPtr)2, IntPtr.Zero);
                return;
            }
        }

        private void WindowBase_SourceInitialized(object sender, EventArgs e)
        {
            Prt = new WindowInteropHelper(this).Handle;
        }
        [DllImport("user32.dll")]

        public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        #endregion

        #region 窗体属性
        /// <summary>
        /// 全屏是否保留任务栏显示
        /// </summary>
        [Description("全屏是否保留任务栏显示")]
        public bool FullScreen
        {
            get { return (bool)GetValue(FullScreenProperty); }
            set { SetValue(FullScreenProperty, value); }
        }
        public static readonly DependencyProperty FullScreenProperty =
            DependencyProperty.Register("FullScreen", typeof(bool), typeof(WindowBase), new PropertyMetadata(false));

        /// <summary>
        /// 窗体阴影大小
        /// </summary>
        [Description("窗体阴影大小")]
        public double WindowShadowSize
        {
            get { return (double)GetValue(WindowShadowSizeProperty); }
            set { SetValue(WindowShadowSizeProperty, value); }
        }
        public static readonly DependencyProperty WindowShadowSizeProperty =
            DependencyProperty.Register("WindowShadowSize", typeof(double), typeof(WindowBase), new PropertyMetadata(10.0));

        /// <summary>
        /// 窗体阴影颜色
        /// </summary>
        [Description("窗体阴影颜色")]
        public Color WindowShadowColor
        {
            get { return (Color)GetValue(WindowShadowColorProperty); }
            set { SetValue(WindowShadowColorProperty, value); }
        }
        public static readonly DependencyProperty WindowShadowColorProperty =
            DependencyProperty.Register("WindowShadowColor", typeof(Color), typeof(WindowBase), new PropertyMetadata(Color.FromArgb(255, 200, 200, 200)));

        /// <summary>
        /// 窗体阴影透明度
        /// </summary>
        [Description("窗体阴影透明度")]
        public double WindowShadowOpacity
        {
            get { return (double)GetValue(WindowShadowOpacityProperty); }
            set { SetValue(WindowShadowOpacityProperty, value); }
        }
        public static readonly DependencyProperty WindowShadowOpacityProperty =
            DependencyProperty.Register("WindowShadowOpacity", typeof(double), typeof(WindowBase), new PropertyMetadata(1.0));
        #endregion
    }
}
