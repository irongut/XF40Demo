using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Convertors
{
    internal class MenuItemToIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }
            switch (((string)value).Trim().ToLower())
            {
                case "home":
                    return "resource://XF40Demo.Resources.home.svg";
                case "bottom tabs":
                    return "resource://XF40Demo.Resources.bottom-left.svg";
                case "top tabs":
                    return "resource://XF40Demo.Resources.top-right.svg";
                case "galactic standings":
                    return "resource://XF40Demo.Resources.pedestal.svg";
                case "galnet news":
                    return "resource://XF40Demo.Resources.file.svg";
                case "mars weather":
                    return "resource://XF40Demo.Resources.thermometer.svg";
                case "settings":
                    return "resource://XF40Demo.Resources.gear.svg";
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
