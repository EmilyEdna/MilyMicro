using Mily.Forms.UI;
using Mily.Forms.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Mily.Forms.Controls
{
    public partial class WindowBase : Window
    {
        public WindowBase()
        {
            InitEvent();
        }

        #region 事件
        IntPtr Prt = IntPtr.Zero;
        private void InitEvent()
        {
            SourceInitialized += WindowBase_SourceInitialized;
            MouseLeftButtonDown += WindowBase_MouseLeftButtonDown;
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
