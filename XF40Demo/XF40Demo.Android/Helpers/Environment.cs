using Android.Content.Res;
using Android.OS;
using Android.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF40Demo.Helpers;
using XF40Demo.Models;

[assembly: Dependency(typeof(XF40Demo.Droid.Helpers.Environment))]
namespace XF40Demo.Droid.Helpers
{
    public class Environment : IEnvironment
    {
        public Theme GetOSTheme()
        {
            // UIMode added in Android 2.2 Froyo API 8
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                UiMode uiModeFlags = Android.App.Application.Context.Resources.Configuration.UiMode & UiMode.NightMask;
                switch (uiModeFlags)
                {
                    case UiMode.NightYes:
                        return Theme.Dark;
                    case UiMode.NightNo:
                        return Theme.Light;
                    default:
                        throw new NotSupportedException($"UiMode {uiModeFlags} not supported");
                }
            }
            else
            {
                return Theme.Light;
            }
        }

        public void SetStatusBarColor(System.Drawing.Color color, bool darkStatusBarTint)
        {
            // Window.SetStatusBarColor added in Android 5.0 Lollipop API 21
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var activity = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
                var window = activity.Window;
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                window.SetStatusBarColor(color.ToPlatformColor());

                // SYSTEM_UI_FLAG_LIGHT_STATUS_BAR added in Android 6.0 Marshmallow API 23
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    const StatusBarVisibility flag = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                    window.DecorView.SystemUiVisibility = darkStatusBarTint ? flag : 0;
                }
            }
        }
    }
}