using Android.App;
using Android.Views;
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
            manager.PathNameForDownloadedFile = new Func<IDownloadFile, string>(File => DownPath() + "CandySugar.apk");
            ((DownloadManagerImplementation)manager).IsVisibleInDownloadsUi = true;
            return manager;
        }
    }
}