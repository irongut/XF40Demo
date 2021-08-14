using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF40Demo.Helpers;
using XF40Demo.Models;
using XF40Demo.Services;

namespace XF40Demo.ViewModels
{
    public class PowerDetailViewModel : BaseViewModel
    {
        public ICommand BackCommand { get; }
        public ICommand JoinDiscordCommand { get; }
        public ICommand OpenRedditCommand { get; }

        #region Properties

        public string CommsLogoFilename
        {
            get
            {
                return string.Equals(PowerStanding.ShortName.Trim(), "aisling", StringComparison.OrdinalIgnoreCase)
                    ? FA5BrandsRegular.Slack
                    : FA5BrandsRegular.Discord;
            }
        }

        public string RedditLogoFilename
        {
            get { return FA5BrandsRegular.RedditSquare; }
        }

        private PowerStanding _powerStanding;
        public PowerStanding PowerStanding
        {
            get { return _powerStanding; }
            set
            {
                if (_powerStanding != value)
                {
                    _powerStanding = value;
                    OnPropertyChanged(nameof(PowerStanding));
                    OnPropertyChanged(nameof(CommsLogoFilename));
                    OnPropertyChanged(nameof(PowerComms));
                }
            }
        }

        private PowerDetails _powerDetails;
        public PowerDetails PowerDetails
        {
            get { return _powerDetails; }
            private set
            {
                if (_powerDetails != value)
                {
                    _powerDetails = value;
                    HasHQEffect = !string.IsNullOrEmpty(_powerDetails.HQSystemEffect);
                    OnPropertyChanged(nameof(PowerDetails));
                }
            }
        }

        private PowerComms _powerComms;
        public PowerComms PowerComms
        {
            get { return _powerComms; }
            private set
            {
                if (_powerComms != value)
                {
                    _powerComms = value;
                    OnPropertyChanged(nameof(PowerComms));
                }
            }
        }

        private bool _commsEnabled;
        public bool CommsEnabled
        {
            get { return _commsEnabled; }
            private set
            {
                if (_commsEnabled != value)
                {
                    _commsEnabled = value;
                    OnPropertyChanged(nameof(CommsEnabled));
                }
            }
        }

        private string _expandText;
        public string ExpandText
        {
            get { return _expandText; }
            set
            {
                if (_expandText != value)
                {
                    _expandText = value;
                    OnPropertyChanged(nameof(ExpandText));
                }
            }
        }

        private string _controlText;
        public string ControlText
        {
            get { return _controlText; }
            set
            {
                if (_controlText != value)
                {
                    _controlText = value;
                    OnPropertyChanged(nameof(ControlText));
                }
            }
        }

        private bool _hasHQEffect;
        public bool HasHQEffect
        {
            get { return _hasHQEffect; }
            private set
            {
                if (_hasHQEffect != value)
                {
                    _hasHQEffect = value;
                    OnPropertyChanged(nameof(HasHQEffect));
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

        #endregion

        public PowerDetailViewModel()
        {
            BackCommand = new Command(async () => await Xamarin.Forms.Shell.Current.GoToAsync("//galacticStandings").ConfigureAwait(false));
            JoinDiscordCommand = new Command(JoinDiscord);
            OpenRedditCommand = new Command(OpenReddit);
        }

        private async Task GetPowerDetailsAsync()
        {
            try
            {
                PowerDetailsService pdService = PowerDetailsService.Instance();
                StandingsService gsService = StandingsService.Instance();
                if (pdService.SelectedPower != null)
                {
                    PowerDetails = pdService.SelectedPower;
                    CancellationTokenSource cancelToken = new CancellationTokenSource();
                    PowerStanding = await gsService.GetPowerAsync(PowerDetails.ShortName, cancelToken).ConfigureAwait(false);
                    Cycle = PowerStanding.Cycle;
                    LastUpdated = PowerStanding.LastUpdated;
                    ExpandText = string.Format("<p>{0}</p>&nbsp;<ul><li>Strong Against: {1}</li><li>Weak Against: {2}</li></ul>",
                                    PowerDetails.ExpansionText,
                                    PowerDetails.ExpansionStrongGovernment,
                                    PowerDetails.ExpansionWeakGovernment);
                    ControlText = string.Format("<p>{0}</p>&nbsp;<ul><li>Strong Against: {1}</li><li>Weak Against: {2}</li></ul>",
                                    PowerDetails.ControlText,
                                    PowerDetails.ControlStrongGovernment,
                                    PowerDetails.ControlWeakGovernment);
                }
            }
            catch (Exception ex)
            {
                ToastHelper.Toast($"Error getting Power details: {ex.Message}");
            }
            await Task.Run(() => GetPowerCommsAsync()).ConfigureAwait(false);
        }

        private async Task GetPowerCommsAsync()
        {
            CommsEnabled = false;
            if (PowerComms == null || PowerComms.ShortName != PowerStanding.ShortName)
            {
                try
                {
                    PowerDetailsService powerService = PowerDetailsService.Instance();
                    PowerComms = await powerService.GetPowerCommsAsync(PowerStanding.ShortName, 3).ConfigureAwait(false);
                }
                catch (HttpRequestException ex)
                {
                    string errorMessage = ex.Message;
                    int start = errorMessage.IndexOf("OPENSSL_internal:", StringComparison.OrdinalIgnoreCase);
                    if (start > 0)
                    {
                        start += 17;
                        int end = errorMessage.IndexOf(" ", start, StringComparison.OrdinalIgnoreCase);
                        errorMessage = $"SSL Error ({errorMessage.Substring(start, end - start).Trim()})";
                    }
                    else if (errorMessage.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        errorMessage = errorMessage.Substring(errorMessage.IndexOf("Error:", StringComparison.OrdinalIgnoreCase) + 6).Trim();
                    }
                    ToastHelper.Toast("Network Error: Unable to download Power comms links");
                }
                catch (Exception)
                {
                    ToastHelper.Toast("Error: Unable to download Power comms links");
                }
            }
            CommsEnabled = PowerComms != null;
        }

        private void JoinDiscord()
        {
            if (PowerComms != null && !string.IsNullOrWhiteSpace(PowerComms.Comms))
            {
                Browser.OpenAsync(new Uri(PowerComms.Comms), BrowserLaunchMode.External);
            }
            else
            {
                ToastHelper.Toast($"Unable to find Power comms for {PowerStanding.ShortName}");
            }
        }

        private void OpenReddit()
        {
            if (PowerComms != null && !string.IsNullOrEmpty(PowerComms.Reddit))
            {
                Browser.OpenAsync(new Uri(PowerComms.Reddit), BrowserLaunchMode.External);
            }
            else
            {
                ToastHelper.Toast($"Unable to find Reddit link for {PowerStanding.ShortName}");
            }
        }

        protected override async void RefreshView()
        {
            await GetPowerDetailsAsync().ConfigureAwait(false);
        }
    }
}
