using CandySugar.App.Controls.ViewModels.AnimeModel;
using CandySugar.Xam.Common;
using CandySugar.Xam.Common.Platform;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Controls.Views.Anime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyAnimePlayView : ContentPage
    {
        public CandyAnimePlayView()
        {
            InitializeComponent();
        }

        private async  void ContentPage_Appearing(object sender, EventArgs e)
        {
            ContainerLocator.Container.Resolve<IAndroidPlatform>().HiddenStatusBar();

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
            var PlayURL = (this.BindingContext as CandyAnimePlayViewModel).PlayURL;
            HtmlWebViewSource Source = new HtmlWebViewSource();
            Source.Html = Extension.ReadLocalHtml("index");
            Source.BaseUrl = Extension.AndroidAssetsPath;
            web.Source = Source;
            await Task.Delay(3000);
            await web.EvaluateJavaScriptAsync($"Play('{PlayURL}')");
        }

        private async void ContentPage_Disappearing(object sender, EventArgs e)
        {
            ContainerLocator.Container.Resolve<IAndroidPlatform>().ShowStatusBar();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
            await web.EvaluateJavaScriptAsync($"Destory()");
        }
    }
}