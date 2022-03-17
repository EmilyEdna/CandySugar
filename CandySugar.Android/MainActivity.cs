using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Lifecycle;
using CandySugar.App;
using CandySugar.App.ViewModels;
using CandySugar.App.Views;
using CandySugar.Droid.Platforms;
using CandySugar.Xam.Common;
using CandySugar.Xam.Common.Platform;
using FFImageLoading.Forms.Platform;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using System.Linq;
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
            //初始化数据库
            SqliteDbContext.Instance.InitTabel();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
    
            CrossCurrentActivity.Current.Activity = this;

            base.OnCreate(savedInstanceState);
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            //初始化XF
            Forms.Init(this, savedInstanceState);
            //初始化图片
            CachedImageRenderer.Init(true);
            //初始化UI库
            Material.Init(this, savedInstanceState);
            //启动
            LoadApplication(new Root(new AndroidInitializer()));
            //屏幕的宽高
            Soft.ScreenWidth = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density;
            Soft.ScreenHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var page = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.FirstOrDefault();
                if (page is CandyIndexView)
                {
                    var view = (CandyIndexViewModel)((CandyIndexView)page).BindingContext;
                    view.RefreshView();
                }
            }

            return base.OnKeyDown(keyCode, e);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IAndroidPlatform, AndroidPlatform>();
        }
    }
}

