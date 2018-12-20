using System;
using System.Globalization;
using System.Windows.Data;

namespace BatteryStatus.WPF.Converters
{
    public class Converter_ExpandedTimeSpan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ret = string.Empty;

            if(value is TimeSpan)
            {
                TimeSpan ts = (TimeSpan)value;
                ret = $"{ts.Hours}H {ts.Minutes}m {ts.Seconds}s";
            }

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
