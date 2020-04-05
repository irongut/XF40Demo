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

        private readonly List<ChartEntry> minWindSpeedEntries;

        private readonly List<ChartEntry> maxWindSpeedEntries;

        private readonly List<ChartEntry> averageWindSpeedEntries;

        private readonly List<ChartEntry> minPressureEntries;

        private readonly List<ChartEntry> maxPressureEntries;

        private readonly List<ChartEntry> averagePressureEntries;

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

        #region Temperature

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

        #region Wind Speed

        private Chart _minWindSpeedChart;
        public Chart MinWindSpeedChart
        {
            get { return _minWindSpeedChart; }
            private set
            {
                if (_minWindSpeedChart != value)
                {
                    _minWindSpeedChart = value;
                    OnPropertyChanged(nameof(MinWindSpeedChart));
                }
            }
        }

        private Chart _maxWindSpeedChart;
        public Chart MaxWindSpeedChart
        {
            get { return _maxWindSpeedChart; }
            private set
            {
                if (_maxWindSpeedChart != value)
                {
                    _maxWindSpeedChart = value;
                    OnPropertyChanged(nameof(MaxWindSpeedChart));
                }
            }
        }

        private Chart _averageWindSpeedChart;
        public Chart AverageWindSpeedChart
        {
            get { return _averageWindSpeedChart; }
            private set
            {
                if (_averageWindSpeedChart != value)
                {
                    _averageWindSpeedChart = value;
                    OnPropertyChanged(nameof(AverageWindSpeedChart));
                }
            }
        }

        #endregion

        #region Pressure

        private Chart _minPressureChart;
        public Chart MinPressureChart
        {
            get { return _minPressureChart; }
            private set
            {
                if (_minPressureChart != value)
                {
                    _minPressureChart = value;
                    OnPropertyChanged(nameof(MinPressureChart));
                }
            }
        }

        private Chart _maxPressureChart;
        public Chart MaxPressureChart
        {
            get { return _maxPressureChart; }
            private set
            {
                if (_maxPressureChart != value)
                {
                    _maxPressureChart = value;
                    OnPropertyChanged(nameof(MaxPressureChart));
                }
            }
        }

        private Chart _averagePressureChart;
        public Chart AveragePressureChart
        {
            get { return _averagePressureChart; }
            private set
            {
                if (_averagePressureChart != value)
                {
                    _averagePressureChart = value;
                    OnPropertyChanged(nameof(AveragePressureChart));
                }
            }
        }

        #endregion

        #endregion

        public WeatherWeekViewModel()
        {
            TemperatureScaleTappedCommand = new Command(ToggleTemperatureScale);
            minTempEntries = new List<ChartEntry>();
            maxTempEntries = new List<ChartEntry>();
            averageTempEntries = new List<ChartEntry>();
            SetupTempCharts();
            minWindSpeedEntries = new List<ChartEntry>();
            maxWindSpeedEntries = new List<ChartEntry>();
            averageWindSpeedEntries = new List<ChartEntry>();
            SetupWindSpeedCharts();
            minPressureEntries = new List<ChartEntry>();
            maxPressureEntries = new List<ChartEntry>();
            averagePressureEntries = new List<ChartEntry>();
            SetupPressureCharts();
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

        private void SetupWindSpeedCharts()
        {
            MinWindSpeedChart = new LineChart()
            {
                Entries = minWindSpeedEntries,
                BackgroundColor = SKColor.Parse(ThemeHelper.GetThemeColor("pageBackgroundColor").ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            MaxWindSpeedChart = new LineChart()
            {
                Entries = maxWindSpeedEntries,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            AverageWindSpeedChart = new LineChart()
            {
                Entries = averageWindSpeedEntries,
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
            MinPressureChart = new LineChart()
            {
                Entries = minPressureEntries,
                BackgroundColor = SKColor.Parse(ThemeHelper.GetThemeColor("pageBackgroundColor").ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            MaxPressureChart = new LineChart()
            {
                Entries = maxPressureEntries,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            AveragePressureChart = new LineChart()
            {
                Entries = averagePressureEntries,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
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
                    MartianDay firstDay = weatherService.Weather.OrderBy(d => d.Sol).First<MartianDay>();
                    MartianDay lastDay = weatherService.Weather.OrderBy(d => d.Sol).Last<MartianDay>();
                    MarsDates = string.Format("Sol {0} - Sol {1}", firstDay.Sol, lastDay.Sol);
                    EarthDates = string.Format("{0:M} - {1:M}", firstDay.FirstUTC, lastDay.FirstUTC);
                    LastUpdated = weatherService.LastUpdated;
                    Season = lastDay.Season;

                    string tempScale = settings.TemperatureScale == TemperatureScale.Celsius ? "°C" : "°F";
                    SetTempChartAxes();
                    SetWindSpeedChartAxes();
                    SetPressureChartAxes();

                    foreach (MartianDay sol in weatherService.Weather.OrderBy(d => d.Sol))
                    {
                        AddTempChartEntries(tempScale, sol);
                        AddWindSpeedChartEntries(sol);
                        AddPressureChartEntries(sol);
                    }
                }
            }
            catch (Exception ex)
            {
                SetMessages(String.Format("Error: {0}", ex.Message));
            }
        }

        private void AddTempChartEntries(string tempScale, MartianDay sol)
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

        private void AddWindSpeedChartEntries(MartianDay sol)
        {
            minWindSpeedEntries.Add(new ChartEntry((float)sol.HorizontalWindSpeed.Min)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Empty,
                Color = SKColor.Parse(Color.DarkGreen.ToHex())
            });

            maxWindSpeedEntries.Add(new ChartEntry((float)sol.HorizontalWindSpeed.Max)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Empty,
                Color = SKColor.Parse(Color.DarkGoldenrod.ToHex())
            });

            averageWindSpeedEntries.Add(new ChartEntry((float)sol.HorizontalWindSpeed.Average)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Format("{0:N1} m/s", sol.HorizontalWindSpeed.Average),
                Color = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex()),
                TextColor = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex())
            });
        }

        private void AddPressureChartEntries(MartianDay sol)
        {
            minPressureEntries.Add(new ChartEntry((float)sol.AtmosphericPressure.Min)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Empty,
                Color = SKColor.Parse(Color.MediumPurple.ToHex())
            });

            maxPressureEntries.Add(new ChartEntry((float)sol.AtmosphericPressure.Max)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Empty,
                Color = SKColor.Parse(Color.DarkBlue.ToHex())
            });

            averagePressureEntries.Add(new ChartEntry((float)sol.AtmosphericPressure.Average)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Format("{0:N0} Pa", sol.AtmosphericPressure.Average),
                Color = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex()),
                TextColor = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex())
            });
        }

        private void SetTempChartAxes()
        {
            double minTemp = weatherService.Weather.OrderBy(t => t.AtmosphericTemp.Min).First<MartianDay>().AtmosphericTemp.Min;
            double maxTemp = weatherService.Weather.OrderBy(t => t.AtmosphericTemp.Max).Last<MartianDay>().AtmosphericTemp.Max;
            MinTempChart.MinValue = (float)minTemp;
            MinTempChart.MaxValue = (float)maxTemp;
            MaxTempChart.MinValue = (float)minTemp;
            MaxTempChart.MaxValue = (float)maxTemp;
            AverageTempChart.MinValue = (float)minTemp;
            AverageTempChart.MaxValue = (float)maxTemp;
        }

        private void SetWindSpeedChartAxes()
        {
            double minWindSpeed = weatherService.Weather.OrderBy(t => t.HorizontalWindSpeed.Min).First<MartianDay>().HorizontalWindSpeed.Min;
            double maxWindSpeed = weatherService.Weather.OrderBy(t => t.HorizontalWindSpeed.Max).Last<MartianDay>().HorizontalWindSpeed.Max;
            MinWindSpeedChart.MinValue = (float)minWindSpeed;
            MinWindSpeedChart.MaxValue = (float)maxWindSpeed;
            MaxWindSpeedChart.MinValue = (float)minWindSpeed;
            MaxWindSpeedChart.MaxValue = (float)maxWindSpeed;
            AverageWindSpeedChart.MinValue = (float)minWindSpeed;
            AverageWindSpeedChart.MaxValue = (float)maxWindSpeed;
        }

        private void SetPressureChartAxes()
        {
            double minPressure = weatherService.Weather.OrderBy(t => t.AtmosphericPressure.Min).First<MartianDay>().AtmosphericPressure.Min;
            double maxPressure = weatherService.Weather.OrderBy(t => t.AtmosphericPressure.Max).Last<MartianDay>().AtmosphericPressure.Max;
            MinPressureChart.MinValue = (float)minPressure;
            MinPressureChart.MaxValue = (float)maxPressure;
            MaxPressureChart.MinValue = (float)minPressure;
            MaxPressureChart.MaxValue = (float)maxPressure;
            AveragePressureChart.MinValue = (float)minPressure;
            AveragePressureChart.MaxValue = (float)maxPressure;
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
