using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace CandySugar.MixUI;

[Activity(Theme = "@style/Maui.MainTheme",
    ConfigurationChanges = ConfigChanges.ScreenSize |
    ConfigChanges.Orientation |
    ConfigChanges.UiMode |
    ConfigChanges.ScreenLayout |
    ConfigChanges.SmallestScreenSize |
    ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Platform.Init(this, savedInstanceState);
        AndroidEnvironment.UnhandledExceptionRaiser += AndroidException;
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    /// <summary>
    /// 全局异常
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AndroidException(object sender, RaiseThrowableEventArgs e)
    {
        var x = e.Exception;

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
