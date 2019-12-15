using Acr.UserDialogs;

namespace XF40Demo.Helpers
{
    public static class ToastHelper
    {
        public static void Toast(string message)
        {
            ToastConfig toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(2000);
            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}