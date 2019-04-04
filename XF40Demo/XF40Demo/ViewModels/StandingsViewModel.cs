﻿using Acr.UserDialogs;
using MonkeyCache.FileStore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            RetryDownloadCommand = new Command(() => GetStandingsAsync(true));
            Standings = new ObservableCollection<PowerStanding>();
        }

        private async void GetStandingsAsync(bool ignoreCache = false)
        {
            ShowMessage = false;
            using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Black))
            {
                try
                {
                    // get the standings
                    List<PowerStanding> standingsList = new List<PowerStanding>();
                    StandingsService standingsService = new StandingsService();
                    (standingsList, Cycle, LastUpdated) = await standingsService.GetData(ignoreCache).ConfigureAwait(false);

                    if (standingsList.Count < 1)
                    {
                        Message = "Unable to display Powerplay Standings due to parsing error.";
                        ShowMessage = true;
                        IsErrorMessage = true;
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
                catch (Exception ex)
                {
                    Message = String.Format("Error downloading Powerplay Standings: {0}", ex.Message);
                    ShowMessage = true;
                    IsErrorMessage = true;
                    return;
                }
            }
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
