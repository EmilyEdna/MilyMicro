using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Mily.Forms.Controls
{
    public class MinSizeBtn: ButtonControl
    {
        Window targetWindow;
        public MinSizeBtn()
        {
            Click += MinSizeBtn_Click;
        }

        private void MinSizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (targetWindow == null)
            {
                targetWindow = Window.GetWindow(this);
            }
            targetWindow.WindowState = WindowState.Minimized;
        }
    }
}
