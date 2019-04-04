using XF40Demo.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class StandingChangeToIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }
            StandingChange change = (StandingChange)value;
            switch (change)
            {
                case StandingChange.up:
                    return "\uf0d8";
                case StandingChange.down:
                    return "\uf0d7";
                default:
                    return String.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
