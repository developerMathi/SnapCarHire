using SnapCarHire.Popups;
using SnapCarHire.Views;
using SnapCarHireController;
using SnapCarHireModel;
using SnapCarHireModel.AccessModels;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace SnapCarHire
{
    public partial class App : Xamarin.Forms.Application
    {
        private ApiToken apiToken;
        private GetClientDetailsForMobileResponse getClientDetailsForMobile;

        public App()
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            Xamarin.Forms.Device.SetFlags(new string[] { "MediaElement_Experimental", "Brush_Experimental", "SwipeView_Experimental" });
            InitializeComponent();

            //var currentVersion = VersionTracking.CurrentVersion;

            if (!App.Current.Properties.ContainsKey("IsOnborded"))
            {
                App.Current.Properties.Add("IsOnborded", false);
            }


            MainPage = new NavigationPage(new Productor());


        }

        protected override void OnStart()
        {
            AppCenter.Start("ios={Your App secret here};" +
                  "uwp={Your UWP App secret here};" +
                  "android={Your Android App secret here};" +
                  "macos={\"8fd4f5c3-d6a9-4502-9c8c-556c68140c36\"};",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
