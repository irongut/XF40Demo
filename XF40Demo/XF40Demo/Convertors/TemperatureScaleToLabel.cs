using System;
using System.Globalization;
using Xamarin.Forms;
using XF40Demo.Models;

namespace XF40Demo.Convertors
{
    internal class TemperatureScaleToLabel : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (TemperatureScale)value == TemperatureScale.Celsius ? "°C" : "°F";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
