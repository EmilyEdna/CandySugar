using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.Core.Content;
using CandySugar.Droid.CrossDownImpl;
using CandySugar.Xam.Common.CrossDownManager;
using CandySugar.Xam.Common.Platform;
using Plugin.CurrentActivity;
using System;
using Env = Android.OS.Environment;

namespace CandySugar.Droid.Platforms
{
    public class AndroidPlatform : IAndroidPlatform
    {
        private Activity Current => CrossCurrentActivity.Current.Activity;
        public void HiddenStatusBar()
        {
            Current.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }
        public void ShowStatusBar()
        {
            Current.Window.ClearFlags(WindowManagerFlags.Fullscreen);
        }
        public void Transparent()
        {
            Current.Window.AddFlags(WindowManagerFlags.TranslucentNavigation | WindowManagerFlags.TranslucentStatus);
        }
        public void ClearTransparent()
        {
            Current.Window.ClearFlags(WindowManagerFlags.TranslucentNavigation | WindowManagerFlags.TranslucentStatus);
        }
        public string DownPath()
        {
            return Env.GetExternalStoragePublicDirectory(Env.DirectoryDownloads).AbsoluteFile + "";
        }
        public IDownloadManager UpdateApk()
        {
            var manager = CrossDownloadManager.Current;
            var filePath = System.IO.Path.Combine(DownPath(), "CandySugar.apk");
            manager.PathNameForDownloadedFile = new Func<IDownloadFile, string>(File => filePath);
            ((DownloadManagerImplementation)manager).IsVisibleInDownloadsUi = true;
            return manager;
        }
        public void InstallApk()
        {
            var filePath = System.IO.Path.Combine(DownPath(), "CandySugar.apk");
            Intent intent = new Intent(Intent.ActionView);
            intent.AddFlags(ActivityFlags.NewTask);
            intent.AddFlags(ActivityFlags.GrantWriteUriPermission);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.AddFlags(ActivityFlags.GrantPersistableUriPermission);
            var jfile = new Java.IO.File(filePath);
            var intentType = "application/vnd.android.package-archive";
            if (Build.VERSION.SdkInt < BuildVersionCodes.N)
                intent.SetDataAndType(Android.Net.Uri.FromFile(jfile), intentType);
            else
            {
                //安卓7.0以上的程序
                var contentUri = FileProvider.GetUriForFile(Current.ApplicationContext, $"{Current.PackageName}.fileprovider", jfile);
                intent.SetDataAndType(contentUri, intentType);
                //安卓8.0以上的程序
                //兼容8.0
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    bool hasInstallPermission = Current.ApplicationContext.PackageManager.CanRequestPackageInstalls();
                    if (!hasInstallPermission)
                    {
                        //注意这个是8.0新API
                        Android.Net.Uri packageURI = Android.Net.Uri.Parse("package:" + Current.PackageName);
                        Intent intents = new Intent(Android.Provider.Settings.ActionManageUnknownAppSources, packageURI);
                        intents.AddFlags(ActivityFlags.NewTask);
                        intents.AddFlags(ActivityFlags.GrantWriteUriPermission);
                        intents.AddFlags(ActivityFlags.GrantReadUriPermission);
                        intents.AddFlags(ActivityFlags.GrantPersistableUriPermission);
                        Current.ApplicationContext.StartActivity(intents);
                        return;
                    }
                }
            }

            Current.ApplicationContext.StartActivity(intent);
        }
    }
}