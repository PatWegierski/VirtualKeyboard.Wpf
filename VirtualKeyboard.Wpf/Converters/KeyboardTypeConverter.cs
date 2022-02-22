using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VirtualKeyboard.Wpf.Types;
using VirtualKeyboard.Wpf.Views;

namespace VirtualKeyboard.Wpf.Converters
{
    internal class KeyboardTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (KeyboardType)value;
            switch (type)
            {
                case KeyboardType.Alphabet: return new AlphabetView();
                case KeyboardType.Special: return new SpecialCharactersView();
                case KeyboardType.NumericOnly: return new NumbersOnlyView();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
