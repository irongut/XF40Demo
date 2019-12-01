using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class FactionToLogo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }
            string faction = ((string)value).Trim().ToLower();
            switch (faction)
            {
                case "alliance":
                    return "resource://XF40Demo.Resources.alliance.green.svg";
                case "empire":
                    return "resource://XF40Demo.Resources.empire.blue.svg";
                case "federation":
                    return "resource://XF40Demo.Resources.federation.red.svg";
                case "independent":
                case "independant": // Uranius can't spell
                    return "resource://XF40Demo.Resources.independent.orange.svg";
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
