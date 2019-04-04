using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class PowerToLogo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }
            string power = ((string)value).Trim().ToLower();
            switch (power)
            {
                case "aisling":
                    return "resource://XF40Demo.Resources.aisling.logo.svg";
                case "delaine":
                    return "resource://XF40Demo.Resources.delaine.logo.svg";
                case "ald":
                    return "resource://XF40Demo.Resources.ald.logo.svg";
                case "patreus":
                    return "resource://XF40Demo.Resources.patreus.logo.svg";
                case "mahon":
                    return "resource://XF40Demo.Resources.mahon.logo.svg";
                case "winters":
                    return "resource://XF40Demo.Resources.winters.logo.svg";
                case "lyr":
                    return "resource://XF40Demo.Resources.lyr.logo.svg";
                case "antal":
                    return "resource://XF40Demo.Resources.antal.logo.svg";
                case "grom":
                    return "resource://XF40Demo.Resources.grom.logo.svg";
                case "hudson":
                    return "resource://XF40Demo.Resources.hudson.logo.svg";
                case "torval":
                    return "resource://XF40Demo.Resources.torval.logo.svg";
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
