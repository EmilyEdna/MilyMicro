using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Mily.Forms.Controls
{
    public class CloseBtn : ButtonControl
    {
        Window targetWindow;
        public CloseBtn()
        {
            ButtonHoverColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            Click += CloseBtn_Click;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (targetWindow == null)
            {
                targetWindow = Window.GetWindow(this);
            }
            targetWindow.Close();
        }
    }
}
