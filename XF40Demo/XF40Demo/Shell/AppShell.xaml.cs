using Xamarin.Forms.Xaml;

namespace XF40Demo.Shell
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
            BindingContext = this;
        }
	}
}