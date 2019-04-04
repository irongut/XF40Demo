using XF40Demo.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class StandingChangeToColour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.Transparent;
            }
            StandingChange change = (StandingChange)value;
            switch (change)
            {
                case StandingChange.up:
                    return Color.LawnGreen;
                case StandingChange.down:
                    return Color.Red;
                default:
                    return Color.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
