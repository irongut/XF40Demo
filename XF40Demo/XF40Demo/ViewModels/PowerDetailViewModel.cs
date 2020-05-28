using System;
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
            BackCommand = new Command(async () => await Xamarin.Forms.Shell.Current.GoToAsync("///galacticStandings").ConfigureAwait(false));
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
                    ExpandText = String.Format("<p>{0}</p>&nbsp;<ul><li>Strong Against: {1}</li><li>Weak Against: {2}</li></ul>",
                                    PowerDetails.ExpansionText,
                                    PowerDetails.ExpansionStrongGovernment,
                                    PowerDetails.ExpansionWeakGovernment);
                    ControlText = String.Format("<p>{0}</p>&nbsp;<ul><li>Strong Against: {1}</li><li>Weak Against: {2}</li></ul>",
                                    PowerDetails.ControlText,
                                    PowerDetails.ControlStrongGovernment,
                                    PowerDetails.ControlWeakGovernment);
                }
            }
            catch (Exception ex)
            {
                ToastHelper.Toast(String.Format("Error getting Power details: {0}", ex.Message));
            }
        }

        private void JoinDiscord()
        {
            string URL = String.Empty;
            switch (_powerStanding.ShortName.Trim().ToLower())
            {
                case "aisling":
                    URL = "https://dinusty.typeform.com/to/ewQU8A";
                    break;
                case "delaine":
                    URL = "https://discord.gg/0nLvLOrzijjO2POL";
                    break;
                case "ald":
                    URL = "https://discord.gg/h28SG5H";
                    break;
                case "patreus":
                    URL = "https://discord.gg/RjWn3qv";
                    break;
                case "mahon":
                    URL = "https://discord.gg/TXYBjgw";
                    break;
                case "winters":
                    URL = "https://discord.gg/8QjHwMF";
                    break;
                case "lyr":
                    URL = "https://discord.gg/0g95XxxKRcw7ypJZ";
                    break;
                case "antal":
                    URL = "https://discord.me/antal";
                    break;
                case "grom":
                    URL = "https://discord.gg/9Uatz63";
                    break;
                case "hudson":
                    URL = "https://discord.gg/YDHTRUM";
                    break;
                case "torval":
                    URL = "https://discord.gg/cj2DgwQ";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(URL))
            {
                Browser.OpenAsync(new Uri(URL), BrowserLaunchMode.External);
            }
        }

        private void OpenReddit()
        {
            string URL = String.Empty;
            switch (_powerStanding.ShortName.Trim().ToLower())
            {
                case "aisling":
                    URL = "https://www.reddit.com/r/aislingduval";
                    break;
                case "delaine":
                    URL = "https://www.reddit.com/r/kumocrew";
                    break;
                case "ald":
                    URL = "https://www.reddit.com/r/elitelavigny/";
                    break;
                case "patreus":
                    URL = "https://www.reddit.com/r/ElitePatreus";
                    break;
                case "mahon":
                    URL = "https://www.reddit.com/r/EliteMahon";
                    break;
                case "winters":
                    URL = "https://www.reddit.com/r/EliteWinters";
                    break;
                case "lyr":
                    URL = "https://www.reddit.com/r/EliteSirius";
                    break;
                case "antal":
                    URL = "https://www.reddit.com/r/EliteAntal";
                    break;
                case "grom":
                    URL = "https://www.reddit.com/r/EliteGrom";
                    break;
                case "hudson":
                    URL = "https://www.reddit.com/r/EliteHudson";
                    break;
                case "torval":
                    URL = "https://www.reddit.com/r/EliteTorval";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(URL))
            {
                Browser.OpenAsync(new Uri(URL), BrowserLaunchMode.External);
            }
        }

        protected override async void RefreshView()
        {
            await GetPowerDetailsAsync().ConfigureAwait(false);
        }
    }
}
