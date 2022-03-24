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
        public CandyKonachanPreView()
        {
            InitializeComponent();
        }

        private void DisAppearinged(object sender, EventArgs e)
        {
            ContainerLocator.Container.Resolve<IAndroidPlatform>().ClearTransparent();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }
    }
}