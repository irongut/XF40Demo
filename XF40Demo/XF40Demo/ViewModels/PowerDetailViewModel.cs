using Acr.UserDialogs;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF40Demo.Models;

namespace XF40Demo.ViewModels
{
    public sealed class PowerDetailViewModel : BaseViewModel
    {
        private static readonly PowerDetailViewModel instance = new PowerDetailViewModel();

        public ICommand BackCommand { get; }
        public ICommand JoinDiscordCommand { get; }
        public ICommand OpenRedditCommand { get; }

        #region Properties

        private PowerStanding _powerStanding;
        public PowerStanding PowerStanding
        {
            get { return _powerStanding; }
            private set
            {
                if (_powerStanding != value)
                {
                    _powerStanding = value;
                    OnPropertyChanged(nameof(PowerStanding));
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

        private readonly List<PowerDetails> powerList;

        private PowerDetailViewModel()
        {
            powerList = new List<PowerDetails>();
            BackCommand = new Command(async () => await Xamarin.Forms.Shell.Current.GoToAsync("///galacticStandings").ConfigureAwait(false));
            JoinDiscordCommand = new Command(JoinDiscord);
            OpenRedditCommand = new Command(OpenReddit);
        }

        public static PowerDetailViewModel Instance()
        {
            return instance;
        }

        public async Task GetPowerDetails(PowerStanding standing)
        {
            PowerStanding = standing;
            Cycle = standing.Cycle;
            LastUpdated = standing.LastUpdated;
            if (powerList?.Any() == false)
            {
                await GetPowerList().ConfigureAwait(false);
            }
            PowerDetails = powerList.Find(x => x.ShortName.Equals(standing.ShortName));
            ExpandText = String.Format("{0}<br/><p>Strong Against: {1}<br/>Weak Against: {2}</p>", _powerDetails.ExpansionText, _powerDetails.ExpansionStrongGovernment, _powerDetails.ExpansionWeakGovernment);
            ControlText = String.Format("{0}<br/><p>Strong Against: {1}<br/>Weak Against: {2}</p>", _powerDetails.ControlText, _powerDetails.ControlStrongGovernment, _powerDetails.ControlWeakGovernment);
        }

        private async Task GetPowerList()
        {
            const string fileName = "XF40Demo.Resources.PowerDetails.csv";

            using (UserDialogs.Instance.Loading("Loading", null, null, true, MaskType.Clear))
            {
                try
                {
                    Assembly assembly = GetType().GetTypeInfo().Assembly;
                    using (Stream stream = assembly.GetManifestResourceStream(fileName))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                            {
                                Delimiter = ","
                            };
                            using (CsvReader csv = new CsvReader(reader, csvConfig))
                            {
                                csv.Read(); // ignore header row
                                while (csv.Read())
                                {
                                    powerList.Add(new PowerDetails(
                                        csv.GetField<int>(0),
                                        csv.GetField<string>(1),
                                        csv.GetField<string>(2),
                                        csv.GetField<int>(3),
                                        csv.GetField<string>(4),
                                        csv.GetField<string>(5),
                                        csv.GetField<string>(6),
                                        csv.GetField<string>(7),
                                        csv.GetField<string>(8),
                                        csv.GetField<string>(9),
                                        csv.GetField<string>(10),
                                        csv.GetField<string>(11),
                                        csv.GetField<string>(12),
                                        csv.GetField<string>(13),
                                        csv.GetField<string>(14),
                                        Desanitise(csv.GetField<string>(15)),
                                        Desanitise(csv.GetField<string>(16)),
                                        Desanitise(csv.GetField<string>(17)),
                                        Desanitise(csv.GetField<string>(18)),
                                        Desanitise(csv.GetField<string>(19)),
                                        Desanitise(csv.GetField<string>(20)),
                                        Desanitise(csv.GetField<string>(21)),
                                        Desanitise(csv.GetField<string>(22)),
                                        Desanitise(csv.GetField<string>(23)),
                                        Desanitise(csv.GetField<string>(24)),
                                        Desanitise(csv.GetField<string>(25))
                                        ));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(String.Format("Error parsing Power Details: {0}", ex.Message), "Elite ALD: Error", "OK").ConfigureAwait(false);
                }
            }
        }

        private string Desanitise(string data)
        {
            return data.Replace("\\n", "\n");
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
                    URL = "https://discord.gg/ZjXwVPy";
                    break;
                case "hudson":
                    URL = "https://discord.gg/YDHTRUM";
                    break;
                case "torval":
                    URL = "https://discord.gg/WXBb784";
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

        protected override void RefreshView()
        {
            // nothing to refresh
        }
    }
}
