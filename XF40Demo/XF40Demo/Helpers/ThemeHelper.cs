using Xamarin.Forms;
using XF40Demo.Models;
using XF40Demo.Services;
using XF40Demo.Styles;

namespace XF40Demo.Helpers
{
    public static class ThemeHelper
    {
        private static readonly SettingsService settings = SettingsService.Instance();

        public static Theme CurrentTheme = settings.ThemeOption;

        public static void ChangeTheme(Theme theme, bool forceTheme = false)
        {
            if ((theme != CurrentTheme) || forceTheme)
            {
                var appResourceDictionary = Application.Current.Resources;
                ResourceDictionary newTheme;
                var environment = DependencyService.Get<IEnvironment>();
                if (theme == Theme.Default)
                {
                    theme = environment?.GetOSTheme() ?? Theme.Light;
                }

                switch (theme)
                {
                    case Theme.Light:
                        newTheme = new LightTheme();
                        break;
                    case Theme.Dark:
                        newTheme = new DarkTheme();
                        break;
                    default:
                        newTheme = new LightTheme();
                        break;
                }

                foreach (var merged in newTheme.MergedDictionaries)
                {
                    appResourceDictionary.MergedDictionaries.Add(merged);
                }

                ManuallyCopyThemes(newTheme, appResourceDictionary);

                CurrentTheme = theme;

                var statusBarColor = (Color)App.Current.Resources["brandColor"];
                environment?.SetStatusBarColor(statusBarColor, true);
            }
        }

        private static void ManuallyCopyThemes(ResourceDictionary fromResource, ResourceDictionary toResource)
        {
            foreach (var item in fromResource.Keys)
            {
                toResource[item] = fromResource[item];
            }
        }
    }
}
