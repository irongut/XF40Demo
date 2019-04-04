using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class CycleImminentToColour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "#7F00FF";
            }
            if ((bool)value)
            {
                return Color.DarkRed;
            }
            else
            {
                return "#7F00FF";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
