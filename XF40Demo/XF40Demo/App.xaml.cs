using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.Helpers;
using XF40Demo.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XF40Demo
{
    public partial class App : Application
    {
        private readonly SettingsService settings = SettingsService.Instance();

        public App()
        {
            InitializeComponent();
            ThemeHelper.ChangeTheme(settings.ThemeOption, true);
            MainPage = new Shell.AppShell();
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
