using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using Xamarin.Forms;
using WatchMeNow.View;
using MediaManager;

namespace WatchMeNow.Droid
{
    [Activity(Label = "WatchMeNow", Icon = "@drawable/AppIcon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossMediaManager.Current.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            MessagingCenter.Subscribe<VideoPage>(this, "LandScape", sender =>
            {
                RequestedOrientation = ScreenOrientation.SensorLandscape;

                var decorView = Window.DecorView;
                var uiOptions = (int)decorView.SystemUiVisibility;
                var newUiOptions = (int)uiOptions;

                newUiOptions |= (int)SystemUiFlags.HideNavigation;
                newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

                decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
            });

            MessagingCenter.Subscribe<VideoPage>(this, "Portrait", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;

                Window.DecorView.SystemUiVisibility = new StatusBarVisibility();

            });

            MessagingCenter.Subscribe<VideoPage>(this, "HideStatusBar", sender =>
             {
                 this.Window.AddFlags(WindowManagerFlags.Fullscreen);
             });

            MessagingCenter.Subscribe<VideoPage>(this, "ShowStatusBar", sender =>
            {
                this.Window.ClearFlags(WindowManagerFlags.Fullscreen);
            });

            FormsVideoPlayer.Init();

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}