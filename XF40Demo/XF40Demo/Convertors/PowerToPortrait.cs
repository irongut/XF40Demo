﻿using System;
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
                    return "resource://XF40Demo.Resources.aisling.headshot.png";
                case "delaine":
                    return "resource://XF40Demo.Resources.delaine.headshot.png";
                case "ald":
                    return "resource://XF40Demo.Resources.ald.headshot.png";
                case "patreus":
                    return "resource://XF40Demo.Resources.patreus.headshot.png";
                case "mahon":
                    return "resource://XF40Demo.Resources.mahon.headshot.png";
                case "winters":
                    return "resource://XF40Demo.Resources.winters.headshot.png";
                case "lyr":
                    return "resource://XF40Demo.Resources.lyr.headshot.png";
                case "antal":
                    return "resource://XF40Demo.Resources.antal.headshot.png";
                case "grom":
                    return "resource://XF40Demo.Resources.grom.headshot.png";
                case "hudson":
                    return "resource://XF40Demo.Resources.hudson.headshot.png";
                case "torval":
                    return "resource://XF40Demo.Resources.torval.headshot.png";
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
