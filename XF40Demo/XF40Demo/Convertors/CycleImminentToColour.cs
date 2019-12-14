using System;
using System.Globalization;
using Xamarin.Forms;
using XF40Demo.Helpers;

namespace XF40Demo.Converters
{
    internal class CycleImminentToColour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return ThemeHelper.GetThemeColor("brandColor");
            }
            if ((bool)value)
            {
                return Color.DarkRed;
            }
            else
            {
                return ThemeHelper.GetThemeColor("brandColor");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
