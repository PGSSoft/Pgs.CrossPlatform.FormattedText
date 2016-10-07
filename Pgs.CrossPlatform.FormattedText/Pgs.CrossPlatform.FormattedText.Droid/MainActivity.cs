﻿
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Text;

namespace Pgs.CrossPlatform.FormattedText.Droid
{
    [Activity(Label = "Pgs.CrossPlatform.FormattedText", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            FormatConfig.Init('[', ']'); // comment when building for NuGet

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

