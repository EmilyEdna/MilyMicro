using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using XExten.XCore;

namespace Mily.Forms.DataModel
{
    public class Commands<T> : ICommand
    {
        private readonly Action<T> _Execute;
        private readonly Func<bool> _CanExecute;
        public Commands(Action<T> Execute, Func<bool> CanExecute)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_CanExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_CanExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute();
        }

        public void Execute(object parameter)
        {
            _Execute((T)parameter);
        }
    }
    public class ObjectConvter : IMultiValueConverter
    {
        private Dictionary<long, string> ConverterObject;
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
