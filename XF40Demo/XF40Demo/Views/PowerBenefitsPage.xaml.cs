using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.ViewModels;

namespace XF40Demo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PowerBenefitsPage : ContentPage
	{
		private readonly PowerDetailViewModel vm = new PowerDetailViewModel();

		public PowerBenefitsPage()
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