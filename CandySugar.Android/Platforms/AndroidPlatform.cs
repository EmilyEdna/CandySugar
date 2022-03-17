using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CandySugar.Xam.Common.Platform;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CandySugar.Droid.Platforms
{
    public class AndroidPlatform : IAndroidPlatform
    {
        private Activity Current => CrossCurrentActivity.Current.Activity;
        public void HiddenStatusBar()
        {
            Current.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }
        public void ShowStatusBar() {
            Current.Window.ClearFlags(WindowManagerFlags.Fullscreen);
        }
    }
}