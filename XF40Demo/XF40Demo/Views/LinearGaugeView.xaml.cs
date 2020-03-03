using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinearGaugeView : Grid
    {
        #region Gauge

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

        public static BindableProperty MinTextProperty = BindableProperty.Create(nameof(MinText), typeof(string), typeof(LinearGaugeView));

        public string MinText
        {
            get { return (string)GetValue(MinTextProperty); }
            set { SetValue(MinTextProperty, value); }
        }

        public static BindableProperty AverageTextProperty = BindableProperty.Create(nameof(AverageText), typeof(string), typeof(LinearGaugeView));

        public string AverageText
        {
            get { return (string)GetValue(AverageTextProperty); }
            set { SetValue(AverageTextProperty, value); }
        }

        public static BindableProperty MaxTextProperty = BindableProperty.Create(nameof(MaxText), typeof(string), typeof(LinearGaugeView));

        public string MaxText
        {
            get { return (string)GetValue(MaxTextProperty); }
            set { SetValue(MaxTextProperty, value); }
        }

        #endregion

        public LinearGaugeView()
        {
            InitializeComponent();
        }
    }
}