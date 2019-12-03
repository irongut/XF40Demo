using XF40Demo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PowerDetailPage : ContentPage
    {
        public PowerDetailPage(PowerDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}