using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CandySugar.App;
using CandySugar.App.ViewModels;
using CandySugar.App.Views;
using CandySugar.Droid.Platforms;
using CandySugar.Xam.Common;
using CandySugar.Xam.Common.DTO;
using CandySugar.Xam.Core.Service;
using FFImageLoading.Forms.Platform;
using MediaManager;
using MediaManager.Player;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Prism.Ioc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using XF.Material.Droid;
using Forms = Xamarin.Forms.Forms;
using Platform = Xamarin.Essentials.Platform;

namespace CandySugar.Droid
{
    [Activity(Theme = "@style/MainTheme",
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize |
        ConfigChanges.Orientation | 
        ConfigChanges.UiMode | 
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //初始化数据库
            SqliteDbContext.Instance.InitTabel();
            //版本号
            Extension.VersionCode = PackageManager.GetPackageInfo(PackageName, 0).VersionName;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CrossCurrentActivity.Current.Activity = this;
            CrossMediaManager.Current.Init();

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

            AndroidEnvironment.UnhandledExceptionRaiser += AndroidException;
        }
        protected override void OnStop()
        {
            base.OnStop();
            if(CrossMediaManager.Current.IsStopped())
                CrossMediaManager.Current.Play();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //权限检查
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var Page = Xamarin.Forms.Application.Current.MainPage;
             
                if (Page is CandyIndexView)
                {
                    var view = (CandyIndexViewModel)((CandyIndexView)Page).BindingContext;
                    view.RefreshView();
                }
            }

            return base.OnKeyDown(keyCode, e);
        }
        /// <summary>
        /// 全局异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AndroidException(object sender, RaiseThrowableEventArgs e)
        {
            ContainerLocator.Container.Resolve<ILoger>().Insert(new CandyGlobalLogDto
            {
                ErrorMsg = e.Exception.Message,
                ErrorStack = e.Exception.StackTrace
            });

            //提示
            Task.Run(() =>
            {
                Looper.Prepare();
                //可以换成更友好的提示
                Toast.MakeText(this, "很抱歉,程序出现异常,即将退出.", ToastLength.Long).Show();
                Looper.Loop();
            });

            //停一会，让前面的操作做完
            Thread.Sleep(2000);

            e.Handled = true;
        }
    }
}

