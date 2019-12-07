using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.ViewModels;

namespace XF40Demo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PowerBenefitsPage : ContentPage
	{
		public PowerBenefitsPage()
        {
            InitializeComponent();
            BindingContext = PowerDetailViewModel.Instance();
        }
    }
}