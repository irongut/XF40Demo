using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XF40Demo.Helpers;
using XF40Demo.Models;
using XF40Demo.Services;

namespace XF40Demo.ViewModels
{
    public class WeatherWeekViewModel : BaseViewModel
    {
        private readonly MarsWeatherService weatherService = MarsWeatherService.Instance();

        #region Properties

        public ICommand TemperatureScaleTappedCommand { get; }

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

        public WeatherWeekViewModel()
        {
            TemperatureScaleTappedCommand = new Command(ToggleTemperatureScale);

        }

        private void ToggleTemperatureScale()
        {
            settings.TemperatureScale = settings.TemperatureScale == TemperatureScale.Celsius ? TemperatureScale.Fahrenheit : TemperatureScale.Celsius;
            foreach (MartianDay sol in weatherService.Weather)
            {
                sol.SetTemperatureScale(settings.TemperatureScale);
            }
        }

        private void SetMessages(string message)
        {
            ToastHelper.Toast(message);
        }

        protected override void RefreshView()
        {
            LastUpdated = DateTime.Now;
        }
    }
}
