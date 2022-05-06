using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls.Converters
{
    public class ButtonContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Flag = parameter.AsString().AsInt();
            if (Flag == 1)
            {
                var Value = value.AsString().AsSpan();
                if (Value.Length > 12)
                    return Value[..12].ToString() + "...";
                else
                    return value;
            }
            else
            {
                var Value = value.AsString().AsSpan();
                if (Value.Length > 10)
                    return Value[..10].ToString() + "...";
                else
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
