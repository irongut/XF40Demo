using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinearGaugeView : Grid
    {
        #region Gauge

        public static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(LinearGaugeView), 0.0);

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static BindableProperty GaugeHeightProperty = BindableProperty.Create(nameof(GaugeHeight), typeof(double), typeof(LinearGaugeView), 30.0);

        public double GaugeHeight
        {
            get { return (double)GetValue(GaugeHeightProperty); }
            set { SetValue(GaugeHeightProperty, value); }
        }

        public static BindableProperty MinColorProperty = BindableProperty.Create(nameof(MinColor), typeof(Color), typeof(LinearGaugeView), Color.Blue);

        public Color MinColor
        {
            get { return (Color)GetValue(MinColorProperty); }
            set { SetValue(MinColorProperty, value); }
        }

        public static BindableProperty MaxColorProperty = BindableProperty.Create(nameof(MaxColor), typeof(Color), typeof(LinearGaugeView), Color.Red);

        public Color MaxColor
        {
            get { return (Color)GetValue(MaxColorProperty); }
            set { SetValue(MaxColorProperty, value); }
        }

        #endregion

        #region Text

        public static BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(LinearGaugeView), FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        public static BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(LinearGaugeView));

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(LinearGaugeView), 16.0);

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LinearGaugeView), Color.White);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static BindableProperty MinProperty = BindableProperty.Create(nameof(Min), typeof(string), typeof(LinearGaugeView));

        public string Min
        {
            get { return (string)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public static BindableProperty AverageProperty = BindableProperty.Create(nameof(Average), typeof(string), typeof(LinearGaugeView));

        public string Average
        {
            get { return (string)GetValue(AverageProperty); }
            set { SetValue(AverageProperty, value); }
        }

        public static BindableProperty MaxProperty = BindableProperty.Create(nameof(Max), typeof(string), typeof(LinearGaugeView));

        public string Max
        {
            get { return (string)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        public static BindableProperty UnitsProperty = BindableProperty.Create(nameof(Units), typeof(string), typeof(FramedTextView));

        public string Units
        {
            get { return (string)GetValue(UnitsProperty); }
            set { SetValue(UnitsProperty, value); }
        }

        #endregion

        public LinearGaugeView()
        {
            InitializeComponent();
        }
    }
}