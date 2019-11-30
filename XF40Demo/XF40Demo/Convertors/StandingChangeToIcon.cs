using XF40Demo.Models;
using System;
using System.Globalization;
using Xamarin.Forms;
using XF40Demo.Helpers;

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
                    return FA5FreeSolid.CaretUp;
                case StandingChange.down:
                    return FA5FreeSolid.CaretDown;
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
