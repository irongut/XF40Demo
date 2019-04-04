using XF40Demo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StandingsPage : ContentPage
    {
        private readonly StandingsViewModel standings = new StandingsViewModel();

        public StandingsPage()
        {
            InitializeComponent();
            BindingContext = standings;
        }

        protected override void OnAppearing()
        {
            standings.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            standings.OnDisappearing();
        }
    }
}