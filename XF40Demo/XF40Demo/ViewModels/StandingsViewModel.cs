using Acr.UserDialogs;
using MonkeyCache.FileStore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
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

        private async void GetStandingsAsync(bool ignoreCache = false)
        {
            ShowMessage = false;
            CancellationTokenSource cancelToken = new CancellationTokenSource();

            using (UserDialogs.Instance.Loading("Loading", () => cancelToken.Cancel(), null, true, MaskType.Clear))
            {
                try
                {
                    // get the standings
                    List<PowerStanding> standingsList = new List<PowerStanding>();
                    StandingsService standingsService = new StandingsService();
                    (standingsList, Cycle, LastUpdated) = await standingsService.GetData(cancelToken, ignoreCache).ConfigureAwait(false);

                    if (standingsList.Count < 1)
                    {
                        SetMessages("Unable to display Powerplay Standings due to parsing error.", true);
                    }
                    else
                    {
                        // show the standings
                        Standings.Clear();
                        foreach (PowerStanding item in standingsList)
                        {
                            Standings.Add(item);
                        }

                        // cache till next cycle if updated
                        int p = _cycle.IndexOf(" ") + 1;
                        Int32.TryParse(_cycle.Substring(p, _cycle.Length - p), out int newCycle);
                        if (newCycle == CycleService.CurrentCycle())
                        {
                            TimeSpan expiry = CycleService.TimeTillTick();
                            string csvText = Barrel.Current.Get<string>(standingsService.DataKey);
                            Barrel.Current.Add(standingsService.DataKey, csvText, expiry);
                            Barrel.Current.Add(standingsService.LastUpdatedKey, _lastUpdated.ToString(), expiry);
                        }
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
                    SetMessages(String.Format("Network Error: {0}", err), true);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("unexpected end of stream"))
                    {
                        SetMessages("Powerplay Standings download was cancelled.", true);
                    }
                    else
                    {
                        SetMessages(String.Format("Error: {0}", ex.Message), true);
                    }
                }
            }
        }

        private async Task OpenPowerDetailsAsync(PowerStanding power)
        {
            PowerDetailViewModel powerDetailViewModel = PowerDetailViewModel.Instance();
            await powerDetailViewModel.GetPowerDetails(power).ConfigureAwait(false);
            await Xamarin.Forms.Shell.Current.GoToAsync("///powerDetails/overview").ConfigureAwait(false);
        }

        private void UpdateTimeRemaining()
        {
            ShowTimeRemaining = !settings.OnlyShowNextCycleWhenImminent || CycleService.CycleImminent();
            TimeRemaining = CycleService.TimeRemaining();
            CycleImminent = CycleService.CycleImminent();
            if (pageVisible && (CycleImminent || DateTime.UtcNow.Minute == 59))
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

        protected override void RefreshView()
        {
            pageVisible = true;
            UpdateTimeRemaining();
            GetStandingsAsync();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            pageVisible = false;
        }
    }
}
