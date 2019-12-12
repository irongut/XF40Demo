using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using XF40Demo.Helpers;
using XF40Demo.Models;

namespace XF40Demo.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public ICommand ResetDefaultsCommand { get; }

        #region Properties

        private bool _wifiOnly;
        public bool WifiOnly
        {
            get { return _wifiOnly; }
            set
            {
                if (_wifiOnly != value)
                {
                    _wifiOnly = value;
                    settings.WifiOnly = value;
                    OnPropertyChanged(nameof(WifiOnly));
                }
            }
        }

        public List<CacheTime> NewsCacheTimes { get; private set; }

        private int _newsCacheSelectedIndex;
        public int NewsCacheSelectedIndex
        {
            get { return _newsCacheSelectedIndex; }
            set
            {
                if (_newsCacheSelectedIndex != value)
                {
                    _newsCacheSelectedIndex = value;
                    settings.NewsCacheTime = NewsCacheTimes[value].Value;
                    OnPropertyChanged(nameof(NewsCacheSelectedIndex));
                }
            }
        }

        public List<string> ThemeOptions { get; }

        private int _themeOptionSelectedIndex;
        public int ThemeOptionSelectedIndex
        {
            get { return _themeOptionSelectedIndex; }
            set
            {
                if (_themeOptionSelectedIndex != value)
                {
                    _themeOptionSelectedIndex = value;
                    settings.ThemeOption = (Theme)value;
                    ThemeHelper.ChangeTheme((Theme)value);
                    OnPropertyChanged(nameof(ThemeOptionSelectedIndex));
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
                    settings.OnlyShowNextCycleWhenImminent = value;
                    OnPropertyChanged(nameof(OnlyShowNextCycleWhenImminent));
                }
            }
        }

        #endregion

        public SettingsViewModel()
        {
            ResetDefaultsCommand = new Command(ResetDefaults);
            ThemeOptions = new List<string>
            {
                "System Default",
                "Light",
                "Dark"
            };
            RefreshView();
        }

        private void ResetDefaults()
        {
            settings.ResetDefault();
            RefreshView();
        }

        protected override void RefreshView()
        {
            ThemeOptionSelectedIndex = (int)settings.ThemeOption;
            WifiOnly = settings.WifiOnly;
            NewsCacheTimes = settings.NewsCacheTimes;
            NewsCacheSelectedIndex = NewsCacheTimes.FindIndex(x => x.Value == settings.NewsCacheTime);
            OnlyShowNextCycleWhenImminent = settings.OnlyShowNextCycleWhenImminent;
        }
    }
}
