using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<ChartEntry> windEntries;

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

        private Chart _windChart;
        public Chart WindChart
        {
            get { return _windChart; }
            private set
            {
                if (_windChart != value)
                {
                    _windChart = value;
                    OnPropertyChanged(nameof(WindChart));
                }
            }
        }

        #endregion

        public WeatherDetailViewModel()
        {
            TemperatureScaleTappedCommand = new Command(ToggleTemperatureScale);
            windEntries = new List<ChartEntry>();
            WindChart = new RadarChart()
            {
                Entries = windEntries,
                BackgroundColor = SKColor.Parse(ThemeHelper.GetThemeColor("pageBackgroundColor").ToHex())
            };
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
                        BuildWindChart();
                    });
                }
            }
            catch (Exception ex)
            {
                SetMessages(String.Format("Error: {0}", ex.Message));
            }
        }

        private void BuildWindChart()
        {
            // radar chart requires all compass points to be added in order
            // but data is not sorted and some compass points may be missing
            windEntries.Clear();
            List<string> compassPoints = new List<string>
            {
                "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"
            };
            foreach (string p in compassPoints)
            {
                CompassPoint point = SolWeather.WindDirection.CompassPoints.Find(x => x.CompassPointName.Equals(p, StringComparison.OrdinalIgnoreCase));
                if (point != null)
                {
                    windEntries.Add(new ChartEntry(point.Count)
                    {
                        Label = point.CompassPointName,
                        Color = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex()),
                        TextColor = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex())
                    });
                }
                else
                {
                    windEntries.Add(new ChartEntry(0)
                    {
                        Label = p,
                        Color = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex()),
                        TextColor = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex())
                    });
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
                await Task.Run(GetSolWeather).ConfigureAwait(false);
            }
        }
    }
}
