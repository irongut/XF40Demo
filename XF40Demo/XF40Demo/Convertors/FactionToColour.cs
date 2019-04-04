using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class FactionToColour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.Transparent;
            }
            string faction = ((string)value).Trim().ToLower();
            switch (faction)
            {
                case "alliance":
                    return "#029E4C";
                case "empire":
                    return "#00B3F7";
                case "federation":
                    return "#FF0000";
                case "independent":
                    return "#FF7100";
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
