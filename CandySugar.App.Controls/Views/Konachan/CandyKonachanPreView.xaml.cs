using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandySugar.Xam.Common.Platform;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Prism.Ioc;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CandySugar.App.Controls.Views.Konachan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandyKonachanPreView : ContentPage
    {
        private readonly IAndroidPlatform platform;
        public CandyKonachanPreView()
        {
            InitializeComponent();
            platform = ContainerLocator.Container.Resolve<IAndroidPlatform>();
        }

        private void Appearinged(object sender, EventArgs e)
        {
            platform.Transparent();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Landscape);
        }

        private void DisAppearinged(object sender, EventArgs e)
        {
            platform.ClearTransparent();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }
    }
}