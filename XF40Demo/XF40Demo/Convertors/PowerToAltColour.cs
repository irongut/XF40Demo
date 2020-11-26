using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class PowerToAltColour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.White;
            }
            switch (((string)value).Trim().ToLower())
            {
                case "delaine":
                case "patreus":
                case "mahon":
                case "lyr":
                case "antal":
                    return Color.Black;
                default:
                    return Color.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
