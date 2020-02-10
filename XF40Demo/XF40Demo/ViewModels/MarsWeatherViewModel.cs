using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XF40Demo.Helpers;
using XF40Demo.Models;
using XF40Demo.Services;

namespace XF40Demo.ViewModels
{
    public class MarsWeatherViewModel : BaseViewModel
    {
        #region Properties

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
            MarsWeather = new ObservableCollection<MartianDay>();
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
                    (weather, LastUpdated) = await weatherService.GetDataAsync(cancelToken, ignoreCache).ConfigureAwait(false);

                    if (weather.Count < 1)
                    {
                        SetMessages("Unable to display Martian Weather due to parsing error.", true);
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
        }
    }
}
