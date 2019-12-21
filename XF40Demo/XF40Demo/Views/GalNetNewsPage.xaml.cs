using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.ViewModels;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalNetNewsPage : ContentPage
    {
        private readonly GalNetNewsViewModel vm = new GalNetNewsViewModel();

        public GalNetNewsPage()
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