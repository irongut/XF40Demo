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
                    return "resource://XF40Demo.Resources.empire.blue.svg";
                case "settings":
                    return "resource://XF40Demo.Resources.engineer.white.svg";
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
