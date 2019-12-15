using MonkeyCache.FileStore;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.Helpers;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public ICommand ToastCommand { get; }

        public MainPage()
        {
            InitializeComponent();
            Barrel.ApplicationId = "com.taranissoftware.XF40Demo";
            ToastCommand = new Command(() => ToastHelper.Toast("It makes TOAST!"));
            BindingContext = this;
        }
    }
}
