using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.ViewModels;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PowerOverviewPage : ContentPage
	{
		public PowerOverviewPage()
        {
            InitializeComponent();
            BindingContext = PowerDetailViewModel.Instance();
        }
    }
}