using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.ViewModels;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("Sol", "sol")]
    public partial class WeatherDetailPage : ContentPage
    {
        private readonly WeatherDetailViewModel vm = new WeatherDetailViewModel();

        private string _sol;
        public string Sol
        {
            get { return _sol; }
            set
            {
                if (_sol != value)
                {
                    _sol = value;
                    uint.TryParse(value, out uint solNo);
                    vm.Sol = solNo;
                    OnPropertyChanged(nameof(Sol));
                }
            }
        }

        public WeatherDetailPage()
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            vm.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            vm.OnDisappearing();
        }
    }
}