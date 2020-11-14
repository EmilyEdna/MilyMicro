using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Mily.Forms.UI.PlayUI
{
    /// <summary>
    /// BangumiFull.xaml 的交互逻辑
    /// </summary>
    public partial class BangumiFull : Window
    {
        public Uri MediaURL { get; set; }
        public BangumiFull()
        {
            InitializeComponent();
        }
        DispatcherTimer timer = null;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bangumi.Source = MediaURL;
            var str = (sender as Button).Content;
            if (str.Equals("播放"))
            {
                Bangumi.Play();
                Bangumi.MediaOpened += (obj, e) =>
                {
                    procss.Maximum = Bangumi.NaturalDuration.TimeSpan.TotalSeconds;
                };
                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                timer.Tick += new EventHandler((obj, e) =>
                {
                    procss.Value = Bangumi.Position.TotalSeconds;
                });
                timer.Start();
            }
            else if (str.Equals("暂停"))
                Bangumi.Pause();
            else
                Bangumi.Stop();
        }
     
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Bangumi.Position = TimeSpan.FromSeconds(procss.Value);
        }

    }
}
