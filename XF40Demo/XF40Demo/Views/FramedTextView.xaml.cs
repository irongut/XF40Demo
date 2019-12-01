using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF40Demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FramedTextView : StackLayout
    {
        #region Border

        public static BindableProperty BorderColourProperty = BindableProperty.Create(nameof(BorderColour), typeof(Color), typeof(FramedTextView), Color.Black);

        public Color BorderColour
        {
            get { return (Color)GetValue(BorderColourProperty); }
            set { SetValue(BorderColourProperty, value); }
        }

        public static BindableProperty BorderRadiusProperty = BindableProperty.Create(nameof(BorderRadius), typeof(float), typeof(FramedTextView));

        public float BorderRadius
        {
            get { return (float)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }

        #endregion

        #region Title

        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(FramedTextView));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static BindableProperty TitleColourProperty = BindableProperty.Create(nameof(TitleColour), typeof(Color), typeof(FramedTextView), Color.Default);

        public Color TitleColour
        {
            get { return (Color)GetValue(TitleColourProperty); }
            set { SetValue(TitleColourProperty, value); }
        }

        public static BindableProperty TitleAttributesProperty = BindableProperty.Create(nameof(TitleAttributes), typeof(FontAttributes), typeof(FramedTextView), FontAttributes.None);

        public FontAttributes TitleAttributes
        {
            get { return (FontAttributes)GetValue(TitleAttributesProperty); }
            set { SetValue(TitleAttributesProperty, value); }
        }
        #endregion

        #region Text

        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FramedTextView));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static BindableProperty TextColourProperty = BindableProperty.Create(nameof(TextColour), typeof(Color), typeof(FramedTextView), Color.Default);

        public Color TextColour
        {
            get { return (Color)GetValue(TextColourProperty); }
            set { SetValue(TextColourProperty, value); }
        }

        public static BindableProperty TextAttributesProperty = BindableProperty.Create(nameof(TextAttributes), typeof(FontAttributes), typeof(FramedTextView), FontAttributes.None);

        public FontAttributes TextAttributes
        {
            get { return (FontAttributes)GetValue(TextAttributesProperty); }
            set { SetValue(TextAttributesProperty, value); }
        }

        #endregion

        public FramedTextView()
        {
            InitializeComponent();
        }
    }
}