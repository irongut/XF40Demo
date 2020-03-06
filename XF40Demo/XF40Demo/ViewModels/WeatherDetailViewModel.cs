using System;
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
        private readonly MarsWeatherService weatherService = MarsWeatherService.Instance();

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
        }

        private void ToggleTemperatureScale()
        {
            settings.TemperatureScale = settings.TemperatureScale == TemperatureScale.Celsius ? TemperatureScale.Fahrenheit : TemperatureScale.Celsius;
            SolWeather.SetTemperatureScale(settings.TemperatureScale);
            foreach (MartianDay sol in weatherService.Weather)
            {
                sol.SetTemperatureScale(settings.TemperatureScale);
            }
        }

        private void GetSolWeather()
        {
            try
            {
                if (weatherService.Weather.Count < 1)
                {
                    SetMessages("No Martian Weather data available.");
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SolWeather = weatherService.Weather.Find(x => x.Sol.Equals(Sol));
                        SolDate = String.Format("Sol {0} - {1:M}", SolWeather.Sol, SolWeather.FirstUTC);
                        LastUpdated = weatherService.LastUpdated;
                    });
                }
            }
            catch (Exception ex)
            {
                SetMessages(String.Format("Error: {0}", ex.Message));
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
