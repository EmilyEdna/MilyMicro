using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Mily.Forms.Utils.Converters
{
    public class FmtConverter : BaseMultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //奇数
            var vals = values.Where((c, i) => i % 2 != 0).ToList();
            //偶数
            var keys = values.Where((c, i) => i % 2 == 0).ToList();
            List<string> result = new List<string>();
            for (int i = 0; i < keys.Count; i++)
            {
                result.Add($"{keys[i]}：{vals[i]}");
            }
            return result.FirstOrDefault();
        }
    }
}
