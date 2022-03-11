﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using CandySugar.App;
using CandySugar.Xam.Common;
using FFImageLoading.Forms.Platform;
using Prism;
using Prism.Ioc;
using Xamarin.Forms.Platform.Android;
using XF.Material.Droid;
using Forms = Xamarin.Forms.Forms;
using Platform = Xamarin.Essentials.Platform;

namespace CandySugar.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            Material.Init(this, savedInstanceState);
            LoadApplication(new Root(new AndroidInitializer()));

            Soft.ScreenWidth = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density;
            Soft.ScreenHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;

            SqliteDbContext.Instance.InitTabel();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

