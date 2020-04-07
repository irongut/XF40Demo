using Rg.Plugins.Popup.Extensions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XF40Demo.ViewModels
{
    public class InSightInfoViewModel : BaseViewModel
    {
        #region Properties

        public INavigation MyNavigation { get; set; }

        public ICommand CloseTappedCommand { get; }

        private double _frameHeight;
        public double FrameHeight
        {
            get { return _frameHeight; }
            private set
            {
                if (_frameHeight != value)
                {
                    _frameHeight = value;
                    OnPropertyChanged(nameof(FrameHeight));
                }
            }
        }

        private string _heroImage;
        public string HeroImage
        {
            get { return _heroImage; }
            private set
            {
                if (_heroImage != value)
                {
                    _heroImage = value;
                    OnPropertyChanged(nameof(HeroImage));
                }
            }
        }

        public string MissionInfo
        {
            get
            {
                StringBuilder mission = new StringBuilder();
                mission.Append("<h2>Mars InSight Mission</h2>");
                mission.Append("<br/>");
                mission.Append("<p>InSight, short for Interior Exploration using Seismic Investigations, Geodesy and Heat Transport, is a Mars lander designed to study the interior of Mars: its crust, mantle, and core. ");
                mission.Append("Studying Mars' interior structure will answer key questions about the early formation of rocky planets in our inner solar system - Mercury, Venus, Earth, and Mars - as well as rocky exoplanets. ");
                mission.Append("InSight measures tectonic activity, internal heat flow, planetary rotation, magnetic disturbances and meteorite impacts on Mars.</p>");
                mission.Append("<br/>");
                mission.Append("<p>The InSight science and engineering team members come from many disciplines, countries and organizations including institutions in the U.S.A., Europe, Japan and the United Kingdom.</p>");
                return mission.ToString();
            }
        }

        public string ApiInfo
        {
            get
            {
                StringBuilder api = new StringBuilder();
                api.Append("<h2>Mars Weather Service API</h2>");
                api.Append("<br/>");
                api.Append("<p>The InSight lander also takes continuous weather measurements - temperature, wind, and air pressure - on the surface of Mars at Elysium Planitia, a flat, smooth plain near the equator.</p>");
                api.Append("<br/>");
                api.Append("<p>The Mars Weather Service API provides per-Sol summary data for each of the last seven available Sols (Martian days). ");
                api.Append("As more data is downlinked from the spacecraft, sometimes several days later, these values are recalculated and consequently may change as more data is received on Earth.</p>");
                api.Append("<br/>");
                api.Append("<p>The API is maintained and provided by NASA Jet Propulsion Laboratory and Cornell University.</p>");
                return api.ToString();
            }
        }

        public string Credits
        {
            get
            {
                StringBuilder credits = new StringBuilder();
                credits.Append("<h2>Credits</h2>");
                credits.Append("<br/>");
                credits.Append("<p>Images are courtesy of NASA / JPL-Caltech.</p>");
                return credits.ToString();
            }
        }

        #endregion

        public InSightInfoViewModel()
        {
            CloseTappedCommand = new Command(async () => await ClosePage().ConfigureAwait(false));
            FrameHeight = DeviceDisplay.MainDisplayInfo.Height * 0.275;
            HeroImage = "resource://XF40Demo.Resources.Mars.insight-patch-600.webp";
        }

        private async Task ClosePage()
        {
            await MyNavigation.PopPopupAsync().ConfigureAwait(false);
        }

        protected override void RefreshView()
        {
            // required by the base vm
        }
    }
}
