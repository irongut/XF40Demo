using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<ChartEntry> averageTempEntries;

        private readonly List<ChartEntry> minTempEntries;

        private readonly List<ChartEntry> maxTempEntries;

        private readonly List<ChartEntry> averageWindSpeedEntries;

        private readonly List<ChartEntry> minWindSpeedEntries;

        private readonly List<ChartEntry> maxWindSpeedEntries;

        private readonly List<ChartEntry> averagePressureEntries;

        private readonly List<ChartEntry> minPressureEntries;

        private readonly List<ChartEntry> maxPressureEntries;

        private bool updatingCharts = false;

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

        #endregion

        #region Wind Speed

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

        #endregion

        #region Pressure

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

        #endregion

        #endregion

        public WeatherWeekViewModel()
        {
            TemperatureScaleTappedCommand = new Command(ToggleTemperatureScale);
            averageTempEntries = new List<ChartEntry>();
            minTempEntries = new List<ChartEntry>();
            maxTempEntries = new List<ChartEntry>();
            SetupTempCharts();
            averageWindSpeedEntries = new List<ChartEntry>();
            minWindSpeedEntries = new List<ChartEntry>();
            maxWindSpeedEntries = new List<ChartEntry>();
            SetupWindSpeedCharts();
            averagePressureEntries = new List<ChartEntry>();
            minPressureEntries = new List<ChartEntry>();
            maxPressureEntries = new List<ChartEntry>();
            SetupPressureCharts();
        }

        private void ToggleTemperatureScale()
        {
            if (!updatingCharts)
            {
                updatingCharts = true;
                try
                {
                    settings.TemperatureScale = settings.TemperatureScale == TemperatureScale.Celsius ? TemperatureScale.Fahrenheit : TemperatureScale.Celsius;
                    foreach (MartianDay sol in weatherService.Weather)
                    {
                        sol.SetTemperatureScale(settings.TemperatureScale);
                    }

                    averageTempEntries.Clear();
                    minTempEntries.Clear();
                    maxTempEntries.Clear();
                    SetTempChartAxes();
                    string tempScale = settings.TemperatureScale == TemperatureScale.Celsius ? "°C" : "°F";
                    foreach (MartianDay sol in weatherService.Weather.OrderBy(d => d.Sol))
                    {
                        sol.SetTemperatureScale(settings.TemperatureScale);
                        AddTempChartEntries(tempScale, sol);
                    }
                }
                catch (Exception ex)
                {
                    SetMessages(String.Format("Error: {0}", ex.Message));
                }
                finally
                {
                    updatingCharts = false;
                }
            }
        }

        private void SetupTempCharts()
        {
            AverageTempChart = new LineChart()
            {
                Entries = averageTempEntries,
                BackgroundColor = SKColor.Parse(ThemeHelper.GetThemeColor("pageBackgroundColor").ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            MinTempChart = new LineChart()
            {
                Entries = minTempEntries,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()),
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
        }

        private void SetupWindSpeedCharts()
        {
            AverageWindSpeedChart = new LineChart()
            {
                Entries = averageWindSpeedEntries,
                BackgroundColor = SKColor.Parse(ThemeHelper.GetThemeColor("pageBackgroundColor").ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            MinWindSpeedChart = new LineChart()
            {
                Entries = minWindSpeedEntries,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()),
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
        }

        private void SetupPressureCharts()
        {
            AveragePressureChart = new LineChart()
            {
                Entries = averagePressureEntries,
                BackgroundColor = SKColor.Parse(ThemeHelper.GetThemeColor("pageBackgroundColor").ToHex()),
                LabelTextSize = 32,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 0,
                LineSize = 8,
                Margin = 20
            };
            MinPressureChart = new LineChart()
            {
                Entries = minPressureEntries,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()),
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
        }

        private void GetWeeklyOverview()
        {
            if (!updatingCharts)
            {
                updatingCharts = true;
                try
                {
                    if (weatherService.Weather.Count < 1)
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

                        SetTempChartAxes();
                        SetWindSpeedChartAxes();
                        SetPressureChartAxes();
                        string tempScale = settings.TemperatureScale == TemperatureScale.Celsius ? "°C" : "°F";

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
                finally
                {
                    updatingCharts = false;
                }
            }
        }

        private void SetTempChartAxes()
        {
            double minTemp = weatherService.Weather.OrderBy(t => t.AtmosphericTemp.Min).First<MartianDay>().AtmosphericTemp.Min;
            double maxTemp = weatherService.Weather.OrderBy(t => t.AtmosphericTemp.Max).Last<MartianDay>().AtmosphericTemp.Max;
            AverageTempChart.MinValue = (float)minTemp;
            AverageTempChart.MaxValue = (float)maxTemp;
            MinTempChart.MinValue = (float)minTemp;
            MinTempChart.MaxValue = (float)maxTemp;
            MaxTempChart.MinValue = (float)minTemp;
            MaxTempChart.MaxValue = (float)maxTemp;
        }

        private void SetWindSpeedChartAxes()
        {
            double maxWindSpeed = weatherService.Weather.OrderBy(t => t.HorizontalWindSpeed.Max).Last<MartianDay>().HorizontalWindSpeed.Max;
            AverageWindSpeedChart.MinValue = 0;
            AverageWindSpeedChart.MaxValue = (float)maxWindSpeed;
            MinWindSpeedChart.MinValue = 0;
            MinWindSpeedChart.MaxValue = (float)maxWindSpeed;
            MaxWindSpeedChart.MinValue = 0;
            MaxWindSpeedChart.MaxValue = (float)maxWindSpeed;
        }

        private void SetPressureChartAxes()
        {
            double minPressure = weatherService.Weather.OrderBy(t => t.AtmosphericPressure.Min).First<MartianDay>().AtmosphericPressure.Min;
            double maxPressure = weatherService.Weather.OrderBy(t => t.AtmosphericPressure.Max).Last<MartianDay>().AtmosphericPressure.Max;
            AveragePressureChart.MinValue = (float)minPressure;
            AveragePressureChart.MaxValue = (float)maxPressure;
            MinPressureChart.MinValue = (float)minPressure;
            MinPressureChart.MaxValue = (float)maxPressure;
            MaxPressureChart.MinValue = (float)minPressure;
            MaxPressureChart.MaxValue = (float)maxPressure;
        }

        private void AddTempChartEntries(string tempScale, MartianDay sol)
        {
            averageTempEntries.Add(new ChartEntry((float)sol.AtmosphericTemp.Average)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Format("{0:N1}{1}", sol.AtmosphericTemp.Average, tempScale),
                Color = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex()),
                TextColor = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex())
            });

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
        }

        private void AddWindSpeedChartEntries(MartianDay sol)
        {
            averageWindSpeedEntries.Add(new ChartEntry((float)sol.HorizontalWindSpeed.Average)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Format("{0:N1} m/s", sol.HorizontalWindSpeed.Average),
                Color = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex()),
                TextColor = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex())
            });

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
        }

        private void AddPressureChartEntries(MartianDay sol)
        {
            averagePressureEntries.Add(new ChartEntry((float)sol.AtmosphericPressure.Average)
            {
                Label = string.Format("{0}", sol.Sol),
                ValueLabel = string.Format("{0:N0} Pa", sol.AtmosphericPressure.Average),
                Color = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex()),
                TextColor = SKColor.Parse(ThemeHelper.GetThemeColor("textColor").ToHex())
            });

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
