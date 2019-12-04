using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.Views;

namespace XF40Demo.Shell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
            BindingContext = this;
            Routing.RegisterRoute("standings/details", typeof(PowerDetailPage));
        }
	}
}