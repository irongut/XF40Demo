using System.ComponentModel;
using Xamarin.Forms;
using XF40Demo.Services;

namespace XF40Demo.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly SettingsService settings = new SettingsService();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnAppearing()
        {
            RefreshView();
            MessagingCenter.Subscribe<App>(this, "AppOnResume", (_) => RefreshView());
        }

        public virtual void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<App>(this, "AppOnResume");
        }

        protected abstract void RefreshView();
    }
}
