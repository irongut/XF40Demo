using Acr.UserDialogs;
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

namespace XF40Demo.ViewModels
{
    public class StandingsViewModel : BaseViewModel
    {
        private bool pageVisible = false;

        #region Properties

        public ICommand PowerTappedCommand { get; }

        public ICommand RetryDownloadCommand { get; }

        public ObservableCollection<PowerStanding> Standings { get; }

        private string _timeRemaining;
        public string TimeRemaining
        {
            get { return _timeRemaining; }
            private set
            {
                if (_timeRemaining != value)
                {
                    _timeRemaining = value;
                    OnPropertyChanged(nameof(TimeRemaining));
                }
            }
        }

        private Color _timeRemainingColor;
        public Color TimeRemainingColor
        {
            get { return _timeRemainingColor; }
            private set
            {
                if (_timeRemainingColor != value)
                {
                    _timeRemainingColor = value;
                    OnPropertyChanged(nameof(TimeRemainingColor));
                }
            }
        }

        private bool _showTimeRemaining;
        public bool ShowTimeRemaining
        {
            get { return _showTimeRemaining; }
            private set
            {
                if (_showTimeRemaining != value)
                {
                    _showTimeRemaining = value;
                    OnPropertyChanged(nameof(ShowTimeRemaining));
                }
            }
        }

        private bool _cycleImminent;
        public bool CycleImminent
        {
            get { return _cycleImminent; }
            private set
            {
                if (_cycleImminent != value)
                {
                    _cycleImminent = value;
                    OnPropertyChanged(nameof(CycleImminent));
                }
            }
        }

        private string _cycle;
        public string Cycle
        {
            get { return _cycle; }
            private set
            {
                if (_cycle != value)
                {
                    _cycle = value;
                    OnPropertyChanged(nameof(Cycle));
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

        public StandingsViewModel()
        {
            PowerTappedCommand = new Command<PowerStanding>(async (x) => await OpenPowerDetailsAsync(x).ConfigureAwait(false));
            RetryDownloadCommand = new Command(() => GetStandingsAsync(true));
            Standings = new ObservableCollection<PowerStanding>();
        }

        private async Task OpenPowerDetailsAsync(PowerStanding power)
        {
            PowerDetailsService pdService = PowerDetailsService.Instance();
            pdService.SetSelectedPower(power.ShortName);
            await Xamarin.Forms.Shell.Current.GoToAsync("//powerDetails/overview").ConfigureAwait(false);
        }

        private async void GetStandingsAsync(bool ignoreCache = false)
        {
            int cycleNo = 0;
            if (!string.IsNullOrWhiteSpace(Cycle))
            {
                int p = Cycle.IndexOf(" ") + 1;
                Int32.TryParse(Cycle.Substring(p, Cycle.Length - p), out cycleNo);
            }

            if ((Standings?.Any() == false) || (cycleNo != CycleService.CurrentCycle() && (LastUpdated + TimeSpan.FromMinutes(10) < DateTime.Now)))
            {
                ShowMessage = false;
                CancellationTokenSource cancelToken = new CancellationTokenSource();

                using (UserDialogs.Instance.Loading("Loading", () => cancelToken.Cancel(), null, true, MaskType.Clear))
                {
                    try
                    {
                        // get the standings
                        StandingsService standingsService = StandingsService.Instance();
                        GalacticStandings standings = await standingsService.GetData(cancelToken, ignoreCache).ConfigureAwait(false);
                        Cycle = $"Cycle {standings.Cycle}";
                        LastUpdated = standings.LastUpdated;

                        if (standings.Standings.Count < 1)
                        {
                            SetMessages("Unable to display Powerplay Standings due to parsing error.", true);
                        }
                        else
                        {
                            // show the standings
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Standings.Clear();
                                foreach (PowerStanding item in standings.Standings)
                                {
                                    Standings.Add(item);
                                }
                            });
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        SetMessages("Powerplay Standings download was cancelled or timed out.", true);
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
                        SetMessages($"Network Error: {err}", true);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("unexpected end of stream"))
                        {
                            SetMessages("Powerplay Standings download was cancelled.", true);
                        }
                        else
                        {
                            SetMessages($"Error: {ex.Message}", true);
                        }
                    }
                }
            }
        }

        private void UpdateTimeRemaining()
        {
            TimeRemaining = CycleService.TimeRemaining();
            CycleImminent = CycleService.CycleImminent();
            if (CycleImminent)
            {
                TimeRemainingColor = Color.DarkRed;
            }
            else
            {
                TimeRemainingColor = ThemeHelper.GetThemeColor("brandColor");
            }
            ShowTimeRemaining = !settings.OnlyShowNextCycleWhenImminent || CycleService.CycleImminent();

            if (pageVisible && (CycleService.FinalDay() || DateTime.UtcNow.Minute == 59))
            {
                Device.StartTimer(TimeSpan.FromSeconds(60 - DateTime.UtcNow.Second), () =>
                {
                    UpdateTimeRemaining();
                    return false;
                });
            }
        }

        private void SetMessages(string message, Boolean isError)
        {
            Message = message;
            ShowMessage = true;
            IsErrorMessage = isError;
        }

        protected override async void RefreshView()
        {
            pageVisible = true;
            UpdateTimeRemaining();
            await Task.Run(() => GetStandingsAsync()).ConfigureAwait(false);
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            pageVisible = false;
        }
    }
}
