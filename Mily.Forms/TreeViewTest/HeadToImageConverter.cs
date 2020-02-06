using Mily.Forms.TreeViewTest.Directory;
using Mily.Forms.TreeViewTest.Directory.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Mily.Forms.TreeViewTest
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeadToImageConverter : IValueConverter
    {
        public static HeadToImageConverter Instance = new HeadToImageConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Icon = "Image/10.png";
            switch ((DirectoryItemType)value)
            {
                case DirectoryItemType.Diver:
                    Icon = "Image/6.png";
                    break;
                case DirectoryItemType.Folder:
                    Icon = "Image/2.png";
                    break;
            }
            var url = new Uri($"pack://application:,,,/{Icon}");
            return new BitmapImage(url);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
