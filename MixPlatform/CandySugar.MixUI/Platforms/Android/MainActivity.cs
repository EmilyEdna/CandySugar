using Android.App;
using Android.Content.PM;
using Android.OS;

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
}
