﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Converters
{
    internal class CycleImminentToColour : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return App.Current.Resources["brandColor"];
            }
            if ((bool)value)
            {
                return Color.DarkRed;
            }
            else
            {
                return App.Current.Resources["brandColor"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
