using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.Helpers;
using XF40Demo.Services;
using XF40Demo.Shell;

[assembly: ExportFont("Font Awesome 5 Brands-Regular-400.otf", Alias = "FontAwesomeBrands")]
[assembly: ExportFont("Font Awesome 5 Free-Solid-900.otf", Alias = "FontAwesomeSolid")]

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XF40Demo
{
    public partial class App : Application
    {
        private readonly SettingsService settings = SettingsService.Instance();

        public App()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental" });
            ThemeHelper.ChangeTheme(settings.ThemeOption, true);
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            MessagingCenter.Send<App>(this, "AppOnResume");
        }
    }
}
