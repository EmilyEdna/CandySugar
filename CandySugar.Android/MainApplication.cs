using System;
using Android.App;
using Android.Runtime;
using Xamarin.Essentials;

namespace CandySugar.Droid
{
    [Application(Theme = "@style/MainTheme")]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Platform.Init(this);
        }
    }
}
