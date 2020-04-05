using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<ChartEntry> minTempEntries;

        private readonly List<ChartEntry> maxTempEntries;

        private readonly List<ChartEntry> averageTempEntries;

        #region Properties

        public ICommand TemperatureScaleTappedCommand { get; }

        private string _marsDates;
        public string MarsDates
        {
            get { return _marsDates; }
            set
            {
                if (_marsDates != value)
                {
                    _marsDates = value;
                    OnPropertyChanged(nameof(MarsDates));
                }
            }
        }

        private string _earthDates;
        public string EarthDates
        {
            get { return _earthDates; }
            set
            {
                if (_earthDates != value)
                {
                    _earthDates = value;
                    OnPropertyChanged(nameof(EarthDates));
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

        private string _season;
        public string Season
        {
            get { return _season; }
            private set
            {
                if (_season != value)
                {
                    _season = value;
                    OnPropertyChanged(nameof(Season));
                }
            }
        }

        private Chart _minTempChart;
        public Chart MinTempChart
        {
            get { return _minTempChart; }
            private set
            {
                if (_minTempChart != value)
                {
                    _minTempChart = value;
                    OnPropertyChanged(nameof(MinTempChart));
                }
            }
        }

        private Chart _maxTempChart;
        public Chart MaxTempChart
        {
            get { return _maxTempChart; }
            private set
            {
                if (_maxTempChart != value)
                {
                    _maxTempChart = value;
                    OnPropertyChanged(nameof(MaxTempChart));
                }
            }
        }

        private Chart _averageTempChart;
        public Chart AverageTempChart
        {
            get { return _averageTempChart; }
            private set
            {
                if (_averageTempChart != value)
                {
                    _averageTempChart = value;
                    OnPropertyChanged(nameof(AverageTempChart));
                }
            }
        }

        #endregion

        public WeatherWeekViewModel()
        {
            TemperatureScaleTappedCommand = new Command(ToggleTemperatureScale);
            minTempEntries = new List<ChartEntry>();
            maxTempEntries = new List<ChartEntry>();
            averageTempEntries = new List<ChartEntry>();
            SetupTempCharts();
            SetupPressureCharts();
            SetupWindSpeedCharts();
        }

        private void ToggleTemperatureScale()
        {
            settings.TemperatureScale = settings.TemperatureScale == TemperatureScale.Celsius ? TemperatureScale.Fahrenheit : TemperatureScale.Celsius;
            foreach (MartianDay sol in weatherService.Weather)
            {
                sol.SetTemperatureScale(settings.TemperatureScale);
            }
        }

        private void SetupTempCharts()
        {
            MinTempChart = new LineChart()
            {
                Entries = minTempEntries,
                BackgroundColor = SKColor.Parse(ThemeHelper.GetThemeColor("pageBackgroundColor").ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            MaxTempChart = new LineChart()
            {
                Entries = maxTempEntries,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            AverageTempChart = new LineChart()
            {
                Entries = averageTempEntries,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
        }

        private void SetupPressureCharts()
        {

        }

        private void SetupWindSpeedCharts()
        {

        }

        private void GetWeeklyOverview()
        {
            try
            {
                if (weatherService.Weather.Count <1)
                {
                    SetMessages("No Martian Weather data available.");
                }
                else
                {
                    // header details
                    MartianDay firstDay = weatherService.Weather.OrderBy(d => d.Sol).First<MartianDay>();
                    MartianDay lastDay = weatherService.Weather.OrderBy(d => d.Sol).Last<MartianDay>();
                    MarsDates = string.Format("Sol {0} - Sol {1}", firstDay.Sol, lastDay.Sol);
                    EarthDates = string.Format("{0:M} - {1:M}", firstDay.FirstUTC, lastDay.FirstUTC);
                    LastUpdated = weatherService.LastUpdated;
                    Season = lastDay.Season;

                    // temperature min / max / scale
                    double minTemp = weatherService.Weather.OrderBy(t => t.AtmosphericTemp.Min).First<MartianDay>().AtmosphericTemp.Min;
                    double maxTemp = weatherService.Weather.OrderBy(t => t.AtmosphericTemp.Max).Last<MartianDay>().AtmosphericTemp.Max;
                    string tempScale = settings.TemperatureScale == TemperatureScale.Celsius ? "°C" : "°F";
                    MinTempChart.MinValue = (float)minTemp;
                    MinTempChart.MaxValue = (float)maxTemp;
                    MaxTempChart.MinValue = (float)minTemp;
                    MaxTempChart.MaxValue = (float)maxTemp;
                    AverageTempChart.MinValue = (float)minTemp;
                    AverageTempChart.MaxValue = (float)maxTemp;

                    // chart values
                    foreach (MartianDay sol in weatherService.Weather.OrderBy(d => d.Sol))
                    {
                        minTempEntries.Add(new ChartEntry((float)sol.AtmosphericTemp.Min)
                        {
                            Label = string.Format("{0}", sol.Sol),
                            ValueLabel = string.Empty,
                            Color = SKColor.Parse(Color.LightBlue.ToHex())
                        });

                        maxTempEntries.Add(new ChartEntry((float)sol.AtmosphericTemp.Max)
                        {
                            Label = string.Format("{0}", sol.Sol),
                            ValueLabel = string.Empty,
                            Color = SKColor.Parse(Color.Red.ToHex())
                        });

                        averageTempEntries.Add(new ChartEntry((float)sol.AtmosphericTemp.Average)
                        {
                            Label = string.Format("{0}", sol.Sol),
                            ValueLabel = string.Format("{0:N1}{1}", sol.AtmosphericTemp.Average, tempScale),
                            Color = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex()),
                            TextColor = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex())
                        });
                    }
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

        protected override void RefreshView()
        {
            GetWeeklyOverview();
        }
    }
}
