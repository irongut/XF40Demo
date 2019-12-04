using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF40Demo.Models;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PowerView : ContentView
    {
        #region Properties

        public static BindableProperty PowerProperty = BindableProperty.Create(nameof(Power), typeof(PowerStanding), typeof(PowerView));

        public PowerStanding Power
        {
            get { return (PowerStanding)GetValue(PowerProperty); }
            set { SetValue(PowerProperty, value); }
        }

        #endregion

        public PowerView ()
        {
            InitializeComponent();
        }
    }
}