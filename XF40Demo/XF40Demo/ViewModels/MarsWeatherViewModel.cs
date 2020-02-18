using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using XF40Demo.Helpers;
using XF40Demo.Models;
using XF40Demo.Services;

namespace XF40Demo.ViewModels
{
    public class MarsWeatherViewModel : BaseViewModel
    {
        #region Properties

        public ICommand TemperatureScaleTappedCommand { get; }

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

        private bool _showMessage;
        public bool ShowMessage
        {
            get { return _showMessage; }
            set
            {
                if (_showMessage != value)
                {
                    _showMessage = value;
                    OnPropertyChanged(nameof(ShowMessage));
                }
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        private bool _isErrorMessage;
        public bool IsErrorMessage
        {
            get { return _isErrorMessage; }
            protected set
            {
                if (_isErrorMessage != value)
                {
                    _isErrorMessage = value;
                    OnPropertyChanged(nameof(IsErrorMessage));
                }
            }
        }

        #endregion

        public MarsWeatherViewModel()
        {
            TemperatureScaleTappedCommand = new Command(ToggleTemperatureScale);
            MarsWeather = new ObservableCollection<MartianDay>();
            TemperatureScale = settings.TemperatureScale;
        }

        private void ToggleTemperatureScale()
        {
            TemperatureScale = TemperatureScale == TemperatureScale.Celsius ? TemperatureScale.Fahrenheit : TemperatureScale.Celsius;
            LatestWeather.SetTemperatureScale(TemperatureScale);
            foreach (MartianDay sol in MarsWeather)
            {
                sol.SetTemperatureScale(TemperatureScale);
            }
        }

        private void SetBackgroundImage()
        {
            switch (settings.MarsBackground)
            {
                case 0:
                    BackgroundImage = "resource://XF40Demo.Resources.Mars.PIA22226-2250.jpg";
                    break;
                case 1:
                    BackgroundImage = "resource://XF40Demo.Resources.Mars.PIA22871-1440.jpg";
                    break;
                case 2:
                    BackgroundImage = "resource://XF40Demo.Resources.Mars.PIA23249-1440.jpg";
                    break;
                case 3:
                    BackgroundImage = "resource://XF40Demo.Resources.Mars.PIA22736-1440.jpg";
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
                    List<MartianDay> weather = new List<MartianDay>();
                    (weather, LastUpdated) = await weatherService.GetDataAsync(TemperatureScale, cancelToken, ignoreCache).ConfigureAwait(false);

                    if (weather.Count < 1)
                    {
                        SetMessages("Unable to display Martian Weather due to data problem.", true);
                    }
                    else
                    {
                        MarsWeather.Clear();
                        foreach (MartianDay day in weather.OrderBy(d => d.Sol))
                        {
                            MarsWeather.Add(day);
                        }
                        LatestWeather = MarsWeather.Last<MartianDay>();
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
                        err = String.Format("SSL Error ({0})", err.Substring(start, end - start).Trim());
                    }
                    else if (err.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        err = err.Substring(err.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) + 6).Trim();
                    }
                    SetMessages(String.Format("Network Error: {0}", err), true);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("unexpected end of stream"))
                    {
                        SetMessages("Martian Weather download was cancelled.", true);
                    }
                    else
                    {
                        SetMessages(String.Format("Error: {0}", ex.Message), true);
                    }
                }
            }
        }

        private void SetMessages(string message, Boolean isError)
        {
            Message = message;
            ShowMessage = true;
            IsErrorMessage = isError;
            ToastHelper.Toast(Message);
        }

        protected override void RefreshView()
        {
            GetWeatherDataAsync();
            SetBackgroundImage();
        }
    }
}
