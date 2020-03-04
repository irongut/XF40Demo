using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF40Demo.Helpers;
using XF40Demo.Models;
using XF40Demo.Services;

namespace XF40Demo.ViewModels
{
    public class WeatherDetailViewModel : BaseViewModel
    {
        #region Properties

        public ICommand TemperatureScaleTappedCommand { get; }

        public uint Sol { get; set; }

        private string _solDate;
        public string SolDate
        {
            get { return _solDate; }
            set
            {
                if (_solDate != value)
                {
                    _solDate = value;
                    OnPropertyChanged(nameof(SolDate));
                }
            }
        }

        private MartianDay _solWeather;
        public MartianDay SolWeather
        {
            get { return _solWeather; }
            private set
            {
                if (_solWeather != value)
                {
                    _solWeather = value;
                    OnPropertyChanged(nameof(SolWeather));
                }
            }
        }

        private TemperatureScale _temperatureScale;
        public TemperatureScale TemperatureScale
        {
            get { return _temperatureScale; }
            set
            {
                if (_temperatureScale != value)
                {
                    _temperatureScale = value;
                    settings.TemperatureScale = value;
                    OnPropertyChanged(nameof(TemperatureScale));
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

        #endregion

        public WeatherDetailViewModel()
        {
            TemperatureScaleTappedCommand = new Command(ToggleTemperatureScale);
            TemperatureScale = settings.TemperatureScale;
        }

        private void ToggleTemperatureScale()
        {
            TemperatureScale = TemperatureScale == TemperatureScale.Celsius ? TemperatureScale.Fahrenheit : TemperatureScale.Celsius;
            SolWeather.SetTemperatureScale(TemperatureScale);
        }

        private async void GetSolWeather()
        {
            CancellationTokenSource cancelToken = new CancellationTokenSource();

            using (UserDialogs.Instance.Loading("Loading", () => cancelToken.Cancel(), null, true, MaskType.Clear))
            {
                try
                {
                    MarsWeatherService weatherService = MarsWeatherService.Instance();
                    List<MartianDay> weather = new List<MartianDay>();
                    (weather, LastUpdated) = await weatherService.GetDataAsync(TemperatureScale, cancelToken, false).ConfigureAwait(false);

                    if (weather.Count < 1)
                    {
                        SetMessages("Unable to display Martian Weather due to data problem.");
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            SolWeather = weather.Find(x => x.Sol.Equals(Sol));
                            SolDate = String.Format("Sol {0} - {1:M}", SolWeather.Sol, SolWeather.FirstUTC);
                        });
                    }
                }
                catch (OperationCanceledException)
                {
                    SetMessages("Martian Weather download was cancelled or timed out.");
                }
                catch (HttpRequestException ex)
                {
                    string err = ex.Message;
                    int start = err.IndexOf("OPENSSL_internal:", StringComparison.OrdinalIgnoreCase);
                    if (start > 0)
                    {
                        start += 17;
                        int end = err.IndexOf(" ", start, StringComparison.OrdinalIgnoreCase);
                        err = String.Format("SSL Error ({0})", err.Substring(start, end - start).Trim());
                    }
                    else if (err.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        err = err.Substring(err.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) + 6).Trim();
                    }
                    SetMessages(String.Format("Network Error: {0}", err));
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("unexpected end of stream"))
                    {
                        SetMessages("Martian Weather download was cancelled.");
                    }
                    else
                    {
                        SetMessages(String.Format("Error: {0}", ex.Message));
                    }
                }
            }
        }

        private void SetMessages(string message)
        {
            ToastHelper.Toast(message);
        }

        protected override async void RefreshView()
        {
            if (Sol > 0 && (SolWeather == null || SolWeather.Sol != Sol))
            {
                await Task.Run(() => GetSolWeather()).ConfigureAwait(false);
            }
        }
    }
}
