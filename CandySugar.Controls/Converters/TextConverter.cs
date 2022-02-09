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
    public class TextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return string.Empty;

            var Flag = parameter.AsString().AsInt();
            if (Flag == 1)
                return value.AsString().IsNullOrEmpty() ? null : $"\t{value.AsString().Replace("　", "\n\t")}";
            else if (Flag == 2)
                return value.AsString().IsNullOrEmpty() ? null : $"\t{value.AsString().Replace("\r\n", "\n\t")}";
            else if(Flag == 3)
                return string.Join(",", (value as List<string>));
            else
            {
                var data = string.Join(",", (value as List<string>));
                return data.Length > 15 ? data.Substring(0, 15) + "..." : data;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
