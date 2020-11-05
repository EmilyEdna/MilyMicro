using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mily.Forms.Controls
{
    public class ButtonControl : Button
    {
        [Description("窗体系统按钮大小")]
        public double ButtonSize
        {
            get { return (double)GetValue(ButtonSizeProperty); }
            set { SetValue(ButtonSizeProperty, value); }
        }
        public static readonly DependencyProperty ButtonSizeProperty =
            DependencyProperty.Register("ButtonSize", typeof(double), typeof(ButtonControl), new PropertyMetadata(30.0));

        [Description("窗体系统按钮鼠标悬浮背景颜色")]
        public SolidColorBrush ButtonHoverColor
        {
            get { return (SolidColorBrush)GetValue(ButtonHoverColorProperty); }
            set { SetValue(ButtonHoverColorProperty, value); }
        }
        public static readonly DependencyProperty ButtonHoverColorProperty =
            DependencyProperty.Register("ButtonHoverColor", typeof(SolidColorBrush), typeof(ButtonControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(50,50,50,50))));

        [Description("窗体系统按钮颜色")]
        public SolidColorBrush ButtonForeground
        {
            get { return (SolidColorBrush)GetValue(ButtonForegroundProperty); }
            set { SetValue(ButtonForegroundProperty, value); }
        }
        public static readonly DependencyProperty ButtonForegroundProperty =
            DependencyProperty.Register("ButtonForeground", typeof(SolidColorBrush), typeof(ButtonControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))));

        [Description("窗体系统按钮鼠标悬按钮颜色")]
        public SolidColorBrush ButtonHoverForeground
        {
            get { return (SolidColorBrush)GetValue(ButtonHoverForegroundProperty); }
            set { SetValue(ButtonHoverForegroundProperty, value); }
        }
        public static readonly DependencyProperty ButtonHoverForegroundProperty =
            DependencyProperty.Register("ButtonHoverForeground", typeof(SolidColorBrush), typeof(ButtonControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255,255,255,255))));

        [Description("图标")]
        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(ButtonControl), new PropertyMetadata(null));
    }
}
