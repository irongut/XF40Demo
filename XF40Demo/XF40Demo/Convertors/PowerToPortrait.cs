using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class PowerToPortrait : IValueConverter
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
                    return "resource://XF40Demo.Resources.aisling.headshot.webp";
                case "delaine":
                    return "resource://XF40Demo.Resources.delaine.headshot.webp";
                case "ald":
                    return "resource://XF40Demo.Resources.ald.headshot.webp";
                case "patreus":
                    return "resource://XF40Demo.Resources.patreus.headshot.webp";
                case "mahon":
                    return "resource://XF40Demo.Resources.mahon.headshot.webp";
                case "winters":
                    return "resource://XF40Demo.Resources.winters.headshot.webp";
                case "lyr":
                    return "resource://XF40Demo.Resources.lyr.headshot.webp";
                case "antal":
                    return "resource://XF40Demo.Resources.antal.headshot.webp";
                case "grom":
                    return "resource://XF40Demo.Resources.grom.headshot.webp";
                case "hudson":
                    return "resource://XF40Demo.Resources.hudson.headshot.webp";
                case "torval":
                    return "resource://XF40Demo.Resources.torval.headshot.webp";
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
