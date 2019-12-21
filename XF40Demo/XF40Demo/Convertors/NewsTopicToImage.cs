using System;
using System.Globalization;
using Xamarin.Forms;

namespace XF40Demo.Convertors
{
    internal class NewsTopicToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return String.Empty;
            }
            switch (((string)value).Trim().ToLower())
            {
                case "unclassified":
                    return "resource://XF40Demo.Resources.galnet.orange.svg";
                case "aliens":
                    return "resource://XF40Demo.Resources.aliens-ufo1.svg";
                case "crime":
                    return "resource://XF40Demo.Resources.crime.svg";
                case "combat":
                    return "resource://XF40Demo.Resources.combat-fighter2.svg";
                case "culture":
                    return "resource://XF40Demo.Resources.theatre.svg";
                case "economy":
                    return "resource://XF40Demo.Resources.economy-chart.svg";
                case "exploration":
                    return "resource://XF40Demo.Resources.exploration-planet.svg";
                case "health":
                    return "resource://XF40Demo.Resources.health.svg";
                case "mining":
                    return "resource://XF40Demo.Resources.mining-pick.svg";
                case "mystery":
                    return "resource://XF40Demo.Resources.mystery.svg";
                case "politics":
                    return "resource://XF40Demo.Resources.politics.svg";
                case "religion":
                    return "resource://XF40Demo.Resources.religion-ankh.svg";
                case "science":
                    return "resource://XF40Demo.Resources.science-atom.svg";
                case "alliance":
                    return "resource://XF40Demo.Resources.alliance.green.svg";
                case "empire":
                    return "resource://XF40Demo.Resources.empire.blue.svg";
                case "federation":
                    return "resource://XF40Demo.Resources.federation.red.svg";
                case "ad":
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
                case "turner":
                case "tarquin":
                case "dekker":
                case "vatermann":
                case "martuuk":
                case "dorn":
                case "farseer":
                case "tani":
                case "ishmaak":
                case "cheung":
                case "ryder":
                case "jameson":
                case "qwent":
                case "hicks":
                case "brandon":
                case "olmanova":
                case "palin":
                case "ram tah":
                case "jean":
                case "dweller":
                case "sarge":
                case "fortune":
                case "blaster":
                case "nemo":
                    return "resource://XF40Demo.Resources.engineer.orange.svg";
                default:
                    return "resource://XF40Demo.Resources.galnet.orange.svg";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
