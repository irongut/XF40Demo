using XF40Demo.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PowerView : ContentView
    {
        #region Properties

        public INavigation MyNavigation { get; set; }

        public static BindableProperty PowerProperty = BindableProperty.Create(nameof(Power), typeof(PowerStanding), typeof(PowerView));

        public PowerStanding Power
        {
            get { return (PowerStanding)GetValue(PowerProperty); }
            set { SetValue(PowerProperty, value); }
        }

        public static BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(PowerView));

        public ICommand TappedCommand
        {
            get { return (ICommand)GetValue(TappedCommandProperty); }
            private set { SetValue(TappedCommandProperty, value); }
        }

        #endregion

        public PowerView ()
        {
            InitializeComponent();
            TappedCommand = new Command(async () => await PowerDetailsAsync().ConfigureAwait(false));
        }

        private async Task PowerDetailsAsync()
        {
            //await MyNavigation.PushAsync(new PowerDetailPage(Power)).ConfigureAwait(false);
        }
    }
}