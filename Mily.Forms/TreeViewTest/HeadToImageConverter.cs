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
            var path = (value as string);
            if (path == null)
                return null;
            var name = Treeview.GetFileOrFolderName(path);
            var Icon = "Image/10.png";
            if (string.IsNullOrEmpty(name))
            {
                Icon = "Image/6.png";
            }
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
            {
                Icon = "Image/2.png";
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
