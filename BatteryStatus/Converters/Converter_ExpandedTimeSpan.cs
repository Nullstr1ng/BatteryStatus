using System;
using Windows.UI.Xaml.Data;

namespace BatteryStatus.Converters
{
    public class Converter_ExpandedTimeSpan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string ret = string.Empty;

            if (value is TimeSpan)
            {
                TimeSpan ts = (TimeSpan)value;
                ret = $"{ts.Hours}H {ts.Minutes}m {ts.Seconds}s";
            }

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
