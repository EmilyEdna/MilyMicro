using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Mily.Forms.Utils.Converters
{
    public class DictionaryConverter : BaseMultiValueConverter 
    {
        private Dictionary<long, string> ConverterObject;
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ConverterObject = new Dictionary<long, string>();
            //奇数
            var vals = values.Where((c, i) => i % 2 != 0).ToList();
            //偶数
            var keys = values.Where((c, i) => i % 2 == 0).ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                ConverterObject.Add(System.Convert.ToInt64(keys[i]), vals[i].ToString());
            }
            return ConverterObject;
        }
    }

}
