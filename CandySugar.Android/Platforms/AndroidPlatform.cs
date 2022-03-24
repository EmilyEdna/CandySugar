using Android.App;
using Android.Views;
using CandySugar.Xam.Common.Platform;
using Plugin.CurrentActivity;
using Env = Android.OS.Environment;
using System.Threading.Tasks;
using XExten.Advance.StaticFramework;
using System.IO;

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
    }
}