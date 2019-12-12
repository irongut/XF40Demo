using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.ViewModels;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        private readonly SettingsViewModel viewModel = new SettingsViewModel();

		public SettingsPage()
		{
			InitializeComponent();
            BindingContext = viewModel;
		}
	}
}