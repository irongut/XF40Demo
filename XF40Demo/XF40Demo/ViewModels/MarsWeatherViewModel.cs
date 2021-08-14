using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF40Demo.Helpers;
using XF40Demo.Models;
using XF40Demo.Services;
using XF40Demo.Views;

namespace XF40Demo.ViewModels
{
    public class MarsWeatherViewModel : BaseViewModel
    {
        #region Properties

        public INavigation MyNavigation { get; set; }

        public ICommand WeeklyTappedCommand { get; }

        public ICommand InfoTappedCommand { get; }

        public ICommand TemperatureScaleTappedCommand { get; }

        public ICommand SolTappedCommand { get; }

        public ObservableCollection<MartianDay> MarsWeather { get; }

        private MartianDay _latestWeather;
        public MartianDay LatestWeather
        {
            get { return _latestWeather; }
            private set
            {
                if (_latestWeather != value)
                {
                    _latestWeather = value;
                    OnPropertyChanged(nameof(LatestWeather));
                }
            }
        }

        private DateTime _lastUpdated;
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            private set
            {
                if (_lastUpdated != value)
                {
                    _lastUpdated = value;
                    OnPropertyChanged(nameof(LastUpdated));
                }
            }
        }

        private string _backgroundImage;
        public string BackgroundImage
        {
            get { return _backgroundImage; }
            private set
            {
                if (_backgroundImage != value)
                {
                    _backgroundImage = value;
                    OnPropertyChanged(nameof(BackgroundImage));
                }
            }
        }

        #endregion

        public MarsWeatherViewModel()
        {
            WeeklyTappedCommand = new Command(async () => await OpenWeatherWeeklyAsync().ConfigureAwait(false));
            InfoTappedCommand = new Command(async () => await ShowInfoAsync().ConfigureAwait(false));
            TemperatureScaleTappedCommand = new Command(ToggleTemperatureScale);
            SolTappedCommand = new Command<uint>(async (x) => await OpenWeatherDetailAsync(x).ConfigureAwait(false));
            MarsWeather = new ObservableCollection<MartianDay>();
        }

        private async Task OpenWeatherWeeklyAsync()
        {
            await Xamarin.Forms.Shell.Current.GoToAsync("//marsWeather/weekly?dummy=0").ConfigureAwait(false);
        }

        private async Task OpenWeatherDetailAsync(uint sol)
        {
            await Xamarin.Forms.Shell.Current.GoToAsync($"//marsWeather/details?sol={sol}").ConfigureAwait(false);
        }

        private async Task ShowInfoAsync()
        {
            await MyNavigation.PushPopupAsync(new InSightInfoPopupPage()).ConfigureAwait(false);
        }

        private void ToggleTemperatureScale()
        {
            settings.TemperatureScale = settings.TemperatureScale == TemperatureScale.Celsius ? TemperatureScale.Fahrenheit : TemperatureScale.Celsius;
            LatestWeather.SetTemperatureScale(settings.TemperatureScale);
            foreach (MartianDay sol in MarsWeather)
            {
                sol.SetTemperatureScale(settings.TemperatureScale);
            }
        }

        private void SetBackgroundImage()
        {
            switch (settings.MarsBackground)
            {
                case 0:
                    BackgroundImage = "resource://XF40Demo.Resources.Mars.PIA22226-2250.webp";
                    break;
                case 1:
                    BackgroundImage = "resource://XF40Demo.Resources.Mars.PIA22871-1440.webp";
                    break;
                case 2:
                    BackgroundImage = "resource://XF40Demo.Resources.Mars.PIA23249-1440.webp";
                    break;
                case 3:
                    BackgroundImage = "resource://XF40Demo.Resources.Mars.PIA22736-1440.webp";
                    break;
                default:
                    BackgroundImage = string.Empty;
                    break;
            }
            if (settings.MarsBackground > 2)
            {
                settings.MarsBackground = DefaultSettings.MarsBackground();
            }
            else
            {
                settings.MarsBackground++;
            }
        }

        private async void GetWeatherDataAsync(bool ignoreCache = false)
        {
            CancellationTokenSource cancelToken = new CancellationTokenSource();

            using (UserDialogs.Instance.Loading("Loading", () => cancelToken.Cancel(), null, true, MaskType.Clear))
            {
                try
                {
                    MarsWeatherService weatherService = MarsWeatherService.Instance();
                    await weatherService.GetDataAsync(cancelToken, ignoreCache).ConfigureAwait(false);

                    if (weatherService.Weather.Count < 1)
                    {
                        SetMessages("Unable to display Martian Weather due to data problem.", true);
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MarsWeather.Clear();
                            foreach (MartianDay day in weatherService.Weather.OrderBy(d => d.Sol))
                            {
                                day.SetTemperatureScale(settings.TemperatureScale);
                                MarsWeather.Add(day);
                            }
                            LatestWeather = MarsWeather.Last<MartianDay>();
                            LastUpdated = weatherService.LastUpdated;
                        });
                    }
                }
                catch (OperationCanceledException)
                {
                    SetMessages("Martian Weather download was cancelled or timed out.", true);
                }
                catch (HttpRequestException ex)
                {
                    string err = ex.Message;
                    int start = err.IndexOf("OPENSSL_internal:", StringComparison.OrdinalIgnoreCase);
                    if (start > 0)
                    {
                        start += 17;
                        int end = err.IndexOf(" ", start, StringComparison.OrdinalIgnoreCase);
                        err = $"SSL Error ({err.Substring(start, end - start).Trim()})";
                    }
                    else if (err.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        err = err.Substring(err.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) + 6).Trim();
                    }
                    SetMessages($"Network Error: {err}", true);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("unexpected end of stream"))
                    {
                        SetMessages("Martian Weather download was cancelled.", true);
                    }
                    else
                    {
                        SetMessages($"Error: {ex.Message}", true);
                    }
                }
            }
        }

        private void SetMessages(string message, bool isError)
        {
            if (isError)
            {
                message = $"Error: {message}";
            }
            ToastHelper.Toast(message);
        }

        protected override void RefreshView()
        {
            GetWeatherDataAsync();
            SetBackgroundImage();
        }
    }
}
