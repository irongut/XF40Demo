using XF40Demo.Models;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XF40Demo.Services
{
    public class SettingsService
    {
        private static readonly SettingsService instance = new SettingsService();
        private const string sharedName = "com.taranissoftware.XF40Demo.settings";

        #region Properties

        private bool _wifiOnly;
        public bool WifiOnly
        {
            get { return _wifiOnly;  }
            set
            {
                if (_wifiOnly != value)
                {
                    _wifiOnly = value;
                    Preferences.Set("wifiOnly", value, sharedName);
                }
            }
        }

        public List<CacheTime> NewsCacheTimes;

        private int _newsCacheTime;
        public int NewsCacheTime
        {
            get { return _newsCacheTime; }
            set
            {
                if (_newsCacheTime != value)
                {
                    _newsCacheTime = value;
                    Preferences.Set("newsCacheTime", value, sharedName);
                }
            }
        }

        private bool _darkTheme;
        public bool DarkTheme
        {
            get { return _darkTheme; }
            set
            {
                if (_darkTheme != value)
                {
                    _darkTheme = value;
                    Preferences.Set("darkTheme", value, sharedName);
                }
            }
        }

        private bool _onlyShowNextCycleWhenImminent;
        public bool OnlyShowNextCycleWhenImminent
        {
            get { return _onlyShowNextCycleWhenImminent; }
            set
            {
                if (_onlyShowNextCycleWhenImminent != value)
                {
                    _onlyShowNextCycleWhenImminent = value;
                    Preferences.Set("onlyShowNextCycleWhenImminent", value, sharedName);
                }
            }
        }

        #endregion

        public SettingsService()
        {
            NewsCacheTimes = new List<CacheTime>
            {
                new CacheTime("1 hour", 1),
                new CacheTime("3 hours", 3),
                new CacheTime("6 hours", 6),
                new CacheTime("9 hours", 9),
                new CacheTime("12 hours", 12)
            };
            LoadAll();
        }

        public static SettingsService Instance()
        {
            return instance;
        }

        public void LoadAll()
        {
            _wifiOnly = Preferences.Get("wifiOnly", DefaultSettings.WifiOnly(), sharedName);
            _newsCacheTime = Preferences.Get("newsCacheTime", DefaultSettings.NewsCacheTime(), sharedName);
            _darkTheme = Preferences.Get("darkTheme", DefaultSettings.DarkTheme(), sharedName);
            _onlyShowNextCycleWhenImminent = Preferences.Get("onlyShowNextCycleWhenImminent", DefaultSettings.OnlyShowNextCycleWhenImminent(), sharedName);
            SetTheme();
        }

        public void ResetDefault()
        {
            Preferences.Clear(sharedName);
            LoadAll();
        }

        public void SetTheme()
        {
            if (App.Current != null)
            {
                if (_darkTheme)
                {
                    App.Current.Resources["backgroundColor"] = Color.FromHex("202125");
                    App.Current.Resources["textColor"] = Color.FromHex("9BA0A6");
                    App.Current.Resources["boldTextColor"] = Color.FromHex("E9EAEE");
                    App.Current.Resources["messageBackgroundColor"] = Color.FromHex("2A2B2F");
                    App.Current.Resources["messageTextColor"] = Color.FromHex("9BA0A6");
                }
                else
                {
                    App.Current.Resources["backgroundColor"] = Color.GhostWhite;
                    App.Current.Resources["textColor"] = Color.Default;
                    App.Current.Resources["boldTextColor"] = Color.FromHex("202125");
                    App.Current.Resources["messageBackgroundColor"] = Color.FromHex("9BA0A6");
                    App.Current.Resources["messageTextColor"] = Color.Default;
                }
            }
        }
    }
}
