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

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            ContainerLocator.Container.Resolve<IAndroidPlatform>().ShowStatusBar();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
            //此段代码因为是异步线程调用会导致先调用WebView先行被释放，而JS还在执行。
            //await web.EvaluateJavaScriptAsync($"Destory()");
        }
    }
}