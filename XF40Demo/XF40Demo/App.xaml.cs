﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XF40Demo
{
    public partial class App : Application
    {
        public App()
        {
            XamEffects.Effects.Init();
            InitializeComponent();
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
