using XF40Demo.Models;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XF40Demo.Services
{
    public class SettingsService
    {
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

        public List<CacheTime> PrioritiesCacheTimes;

        private int _prioritiesCacheTime;
        public int PrioritiesCacheTime
        {
            get { return _prioritiesCacheTime; }
            set
            {
                if (_prioritiesCacheTime != value)
                {
                    _prioritiesCacheTime = value;
                    Preferences.Set("prioritiesCacheTime", value, sharedName);
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

        private bool _showEliteStatusOnMenu;
        public bool ShowEliteStatusOnMenu
        {
            get { return _showEliteStatusOnMenu; }
            set
            {
                if (_showEliteStatusOnMenu != value)
                {
                    _showEliteStatusOnMenu = value;
                    Preferences.Set("showEliteStatusOnMenu", value, sharedName);
                }
            }
        }

        private bool _copySystemName;
        public bool CopySystemName
        {
            get { return _copySystemName; }
            set
            {
                if (_copySystemName != value)
                {
                    _copySystemName = value;
                    Preferences.Set("copySystemName", value, sharedName);
                }
            }
        }

        private bool _sendCrashReports;
        public bool SendCrashReports
        {
            get { return _sendCrashReports; }
            set
            {
                if (_sendCrashReports != value)
                {
                    _sendCrashReports = value;
                    Preferences.Set("sendCrashReports", value, sharedName);
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
            PrioritiesCacheTimes = new List<CacheTime>
            {
                new CacheTime("5 minutes", 5),
                new CacheTime("10 minutes", 10),
                new CacheTime("15 minutes", 15),
                new CacheTime("30 minutes", 30)
            };
            LoadAll();
        }

        public void LoadAll()
        {
            _wifiOnly = Preferences.Get("wifiOnly", DefaultSettings.WifiOnly(), sharedName);
            _newsCacheTime = Preferences.Get("newsCacheTime", DefaultSettings.NewsCacheTime(), sharedName);
            _prioritiesCacheTime = Preferences.Get("prioritiesCacheTime", DefaultSettings.PrioritiesCacheTime(), sharedName);
            _darkTheme = Preferences.Get("darkTheme", DefaultSettings.DarkTheme(), sharedName);
            _onlyShowNextCycleWhenImminent = Preferences.Get("onlyShowNextCycleWhenImminent", DefaultSettings.OnlyShowNextCycleWhenImminent(), sharedName);
            _showEliteStatusOnMenu = Preferences.Get("showEliteStatusOnMenu", DefaultSettings.ShowEliteStatusOnMenu(), sharedName);
            _copySystemName = Preferences.Get("copySystemName", DefaultSettings.CopySystemName(), sharedName);
            _sendCrashReports = Preferences.Get("sendCrashReports", DefaultSettings.SendCrashReports(), sharedName);
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
                    App.Current.Resources["backgroundColor"] = Color.FromHex("#2F3136");
                    App.Current.Resources["textColor"] = Color.LightGray;
                    App.Current.Resources["messageBackgroundColor"] = Color.FromHex("#484B51");
                    App.Current.Resources["messageTextColor"] = Color.LightGray;
                }
                else
                {
                    App.Current.Resources["backgroundColor"] = Color.White;
                    App.Current.Resources["textColor"] = Color.Default;
                    App.Current.Resources["messageBackgroundColor"] = Color.LightGray;
                    App.Current.Resources["messageTextColor"] = Color.Default;
                }
            }
        }
    }
}
