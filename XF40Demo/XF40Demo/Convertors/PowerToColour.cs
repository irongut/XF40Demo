using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class PowerToColour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.Transparent;
            }
            string power = ((string)value).Trim().ToLower();
            switch (power)
            {
                case "aisling":
                    return "#0099FF";
                case "delaine":
                    return "#7FFF00";
                case "ald":
                    return "#7F00FF";
                case "patreus":
                    return "#00FFFF";
                case "mahon":
                    return "#00FF00";
                case "winters":
                    return "#FF7F00";
                case "lyr":
                    return "#BFFF00";
                case "antal":
                    return "#FFFF00";
                case "grom":
                    return "#FF4000";
                case "hudson":
                    return "#FF0000";
                case "torval":
                    return "#0040FF";
                default:
                    return Color.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
