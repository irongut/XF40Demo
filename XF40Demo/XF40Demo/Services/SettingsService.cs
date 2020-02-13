using System.Collections.Generic;
using Xamarin.Essentials;
using XF40Demo.Models;

namespace XF40Demo.Services
{
    public sealed class SettingsService
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

        private Theme _themeOption;
        public Theme ThemeOption
        {
            get { return _themeOption; }
            set
            {
                if (_themeOption != value)
                {
                    _themeOption = value;
                    Preferences.Set("themeOption", (int)value, sharedName);
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

        private int _marsBackground;
        public int MarsBackground
        {
            get { return _marsBackground; }
            set
            {
                if (_marsBackground != value)
                {
                    _marsBackground = value;
                    Preferences.Set("marsBackground", value, sharedName);
                }
            }
        }

        #endregion

        private SettingsService()
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
            _themeOption = (Theme)Preferences.Get("themeOption", DefaultSettings.ThemeOption(), sharedName);
            _onlyShowNextCycleWhenImminent = Preferences.Get("onlyShowNextCycleWhenImminent", DefaultSettings.OnlyShowNextCycleWhenImminent(), sharedName);
            _marsBackground = Preferences.Get("marsBackground", DefaultSettings.MarsBackground(), sharedName);
        }

        public void ResetDefault()
        {
            Preferences.Clear(sharedName);
            LoadAll();
        }
    }
}
