using Android.App;
using Android.Content;

namespace CandySugar.MixUI.Platforms.Android
{
    [Activity(Theme = "@style/CustomerStyle", MainLauncher = true, NoHistory = true)]
    public class LancherActivity : MauiAppCompatActivity
    {
        protected async override void OnResume()
        {
            base.OnResume();
            await Task.Delay(3000);
            StartActivity(new Intent(Application.ApplicationContext, typeof(MainActivity)));
        }
    }
}
