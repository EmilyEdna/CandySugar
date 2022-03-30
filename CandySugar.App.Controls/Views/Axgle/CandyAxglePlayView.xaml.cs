using CandySugar.Xam.Common.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using CandySugar.App.Controls.ViewModels.AxgleModel;
using CandySugar.Xam.Common;

namespace CandySugar.App.Controls.Views.Axgle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyAxglePlayView : ContentPage
    {
        public CandyAxglePlayView()
        {
            InitializeComponent();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            ContainerLocator.Container.Resolve<IAndroidPlatform>().HiddenStatusBar();

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
            var PlayURL = (this.BindingContext as CandyAxglePlayViewModel).PlayURL;
            HtmlWebViewSource Source = new HtmlWebViewSource();
            Source.Html = Extension.ReadLocalHtml("axgle");
            web.Source = Source;
            await Task.Delay(3000);
            await web.EvaluateJavaScriptAsync($"Init('{PlayURL}')");
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            ContainerLocator.Container.Resolve<IAndroidPlatform>().ShowStatusBar();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }
    }
}