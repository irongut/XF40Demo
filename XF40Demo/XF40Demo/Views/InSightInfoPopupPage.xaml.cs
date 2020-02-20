using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using XF40Demo.ViewModels;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InSightInfoPopupPage : PopupPage
    {
        private readonly InSightInfoViewModel vm = new InSightInfoViewModel();

        public InSightInfoPopupPage()
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            // return base.OnBackgroundClicked();
            return false;
        }
    }
}